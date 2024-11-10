using Automate.Services.Commands;
using Automate.Utils.Constants;
using System.Windows.Input;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Automate.Services;
using System.Collections.Generic;
using Automate.Models;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

public class CalendarViewModel
{
    private readonly string selectDateErrorMessage = "Veuillez sélectionner une date dans le calendrier.";
    private readonly string selectEventTitleErrorMessage = "Veuillez sélectionner un événement à modifier.";
    private readonly string errorTitle = "Erreur";
    private readonly string noEvenTitle = "Aucun événement";

    public CalendarCommand CalendarCommand { get; }
    public ICommand OnAddEventClick { get; }
    public ICommand OnEditEventClick { get; }
    public ICommand OnDeleteEventClick { get; }
    public ICommand OnMonthChanged { get; }
    public ICommand ClickOnDate { get; }
    public Calendar Calendar { get; set; }
    public ObservableCollection<string> EventTitles { get; set; } = new ObservableCollection<string>();

    public DateTime? SelectedDate { get; set; }

    public string? SelectedEventTitle { get; set; }

    private readonly TaskCRUDService taskService;

    public CalendarViewModel(Calendar calendar)
    {
        Calendar = calendar;

        var mongoDBService = new MongoDBServices(DBConstants.DB_NAME);
        taskService = new TaskCRUDService(mongoDBService);

        CalendarCommand = new CalendarCommand(taskService);

        OnAddEventClick = new RelayCommand(AddEvent);
        OnEditEventClick = new RelayCommand(EditEvent);
        OnDeleteEventClick = new RelayCommand(DeleteEvent);
        ClickOnDate = new RelayCommand(ClickEvent);
        OnMonthChanged = new RelayCommand(HighlightEventDates);
        
        HighlightEventDates();
        ShowTaskDetails(DateTime.Today);
    }

    private void ClickEvent()
    {
        if (ValidateSelectedDate())
            ShowTaskDetails(SelectedDate!.Value);
    }

    private void AddEvent()
    {
        if (!ValidateSelectedDate())
            return;

        CalendarCommand.AddTask(SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    private void EditEvent()
    {
        if (!ValidateSelectedDate() || !ValidateSelectedEventTitle())
            return;

        CalendarCommand.EditTask(SelectedEventTitle!, SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    private void DeleteEvent()
    {
        if (!ValidateSelectedDate())
            return;

        CalendarCommand.DeleteTask(SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    public void ShowTaskDetails(DateTime selectedDate)
    {
        EventTitles.Clear();
        List<UpcomingTask> tasks = taskService.GetTasksByDate(selectedDate);

        if (tasks.Count > 0)
        {
            foreach (var task in tasks)
                EventTitles.Add(task.Title.ToString());
        }
        else
            EventTitles.Add(noEvenTitle);
    }

    private void HighlightEventDates()
    {
        foreach (var calendarDayButton in FindVisualChildren<CalendarDayButton>(Calendar))
        {
            if (calendarDayButton.DataContext is DateTime date)
            {
                List<UpcomingTask> tasks = taskService.GetTasksByDate(date);
                calendarDayButton.Background = 
                    tasks.Count > 0 ? new SolidColorBrush(Colors.LightCoral) : new SolidColorBrush(Colors.Transparent);
            }
        }
    }

    // Faite avec ai, mais accepté par Laurent
    private static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        var results = new List<T>();

        if (depObj != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T tChild) results.Add(tChild);
                results.AddRange(FindVisualChildren<T>(child));
            }
        }

        return results;
    }

    private bool ValidateSelectedDate()
    {
        if (SelectedDate == null)
        {
            MessageBox.Show(selectDateErrorMessage, errorTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    private bool ValidateSelectedEventTitle()
    {
        if (SelectedEventTitle == null)
        {
            MessageBox.Show(selectEventTitleErrorMessage, errorTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }
}
