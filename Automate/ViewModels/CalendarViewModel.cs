using Automate.Services.Commands;
using Automate.Utils.Constants;
using System.Windows.Input;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Automate.Models;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Environment = Automate.Utils.Environment;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Automate.Utils.Enums;
using Automate.Utils.Validation;
using System.Collections;
using Automate.Views;
using MongoDB.Driver;
using Automate.Abstract.Services;

public class CalendarViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly string selectDateErrorMessage = "Veuillez sélectionner une date dans le calendrier.";
    private readonly string selectEventTitleErrorMessage = "Veuillez sélectionner un événement à modifier.";
    private readonly string noEvenTitle = "Aucun événement";

    private readonly ITasksServices tasksServices;

    private ErrorsCollection errorsCollection;

    public ICommand OnAddEventClick { get; }
    public ICommand OnEditEventClick { get; }
    public ICommand OnDeleteEventClick { get; }
    public ICommand OnMonthChanged { get; }
    public ICommand ClickOnDate { get; }

    public bool HasErrors => errorsCollection.ContainsAnyError();
    public string ErrorMessages
    {
        get => errorsCollection.GetAllErrorMessages();
    }

    public DateTime? SelectedDate { get; set; }
    public string? SelectedEventTitle { get; set; }
    public Calendar Calendar { get; set; }
    public ObservableCollection<string> EventTitles { get; set; } = new ObservableCollection<string>();

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool IsAdmin 
    {
        get => Environment.authenticatedUser.Role == RoleConstants.ADMIN;
    }

    public CalendarViewModel(Calendar calendar, ITasksServices tasksServices)
    {
        Calendar = calendar;
        this.tasksServices = tasksServices;

        errorsCollection = new ErrorsCollection(ErrorsChanged);

        OnAddEventClick = new RelayCommand(AddEvent);
        OnEditEventClick = new RelayCommand(EditEvent);
        OnDeleteEventClick = new RelayCommand(DeleteEvent);
        ClickOnDate = new RelayCommand(ClickEvent);
        OnMonthChanged = new RelayCommand(HighlightEventDates);
        
        HighlightEventDates();
        ShowTaskDetails(DateTime.Today);
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        AddTask(SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    private void EditEvent()
    {
        if (!ValidateSelectedDate() || !ValidateSelectedEventTitle())
            return;

        EditTask(SelectedEventTitle!, SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    private void DeleteEvent()
    {
        if (!ValidateSelectedDate())
            return;

        DeleteTask(SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    public void ShowTaskDetails(DateTime selectedDate)
    {
        EventTitles.Clear();
        List<UpcomingTask> tasks = tasksServices.GetTasksByDate(selectedDate);

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
                List<UpcomingTask> tasks = tasksServices.GetTasksByDate(date);
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

    private void AddTask(DateTime taskDate)
    {
        var eventForm = new TaskFormWindow(taskDate);
        var result = eventForm.ShowDialog();

        if (result == true)
        {
            var newTask = new UpcomingTask
            {
                Title = (EventType)eventForm.taskFormViewModel.SelectedEventType!,
                EventDate = taskDate
            };

            tasksServices.CreateTask(newTask);

            MessageBox.Show(
                $"Événement '{eventForm.taskFormViewModel.SelectedEventType}' ajouté pour le {taskDate.ToShortDateString()}");
        }
    }

    private void EditTask(string taskTitle, DateTime taskDate)
    {
        List<UpcomingTask> tasks = tasksServices.GetTasksByDate(taskDate);
        UpcomingTask? existingTask = tasks.Find(task => task.EventDate.Date == taskDate && task.Title.ToString() == taskTitle);

        if (existingTask != null)
        {
            HandleEditForm(existingTask, taskDate);
        }
        else
        {
            MessageBox.Show("Aucun événement à modifier pour cette date.", "Erreur",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void HandleEditForm(UpcomingTask existingTask, DateTime taskDate)
    {
        var eventForm = new TaskFormWindow(taskDate, existingTask.Title);
        var result = eventForm.ShowDialog();

        if (result == true)
        {
            var updateDefinition = Builders<UpcomingTask>.Update
                .Set(t => t.Title, eventForm.taskFormViewModel.SelectedEventType)
                .Set(t => t.EventDate, taskDate);
            tasksServices.UpdateTask(existingTask.Id, updateDefinition);

            MessageBox.Show(
                $"Événement '{eventForm.taskFormViewModel.SelectedEventType}' modifié pour le {taskDate.ToShortDateString()}");
        }
    }

    public void DeleteTask(DateTime taskDate)
    {
        if (!AskForDeletion(taskDate))
            return;

        List<UpcomingTask> tasks = tasksServices.GetTasksByDate(taskDate);
        UpcomingTask? taskToDelete = tasks.Find(t => t.EventDate.Date == taskDate.Date);

        if (taskToDelete != null)
        {
            tasksServices.DeleteTask(taskToDelete.Id);
            tasks.Remove(taskToDelete);

            MessageBox.Show("Événement supprimé avec succès.");
        }
        else
        {
            MessageBox.Show("Aucun événement trouvé à cette date.", "Erreur",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private bool AskForDeletion(DateTime taskDate)
    {
        var result = MessageBox.Show(
            $"Voulez-vous vraiment supprimer l'événement du {taskDate.ToShortDateString()} ?",
            "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        return result == MessageBoxResult.Yes;
    }

    private bool ValidateSelectedDate()
    {
        return CommonValidation.ValidateNull(
            nameof(SelectedEventTitle),
            SelectedDate,
            selectDateErrorMessage,
            errorsCollection,
            NotifyErrorChange
        );
    }

    private bool ValidateSelectedEventTitle()
    {
        return CommonValidation.ValidateNull(
            nameof(SelectedEventTitle),
            SelectedEventTitle,
            selectEventTitleErrorMessage,
            errorsCollection,
            NotifyErrorChange
        );
    }

    private void NotifyErrorChange()
    {
        OnPropertyChanged(nameof(ErrorMessages));
    }

    public IEnumerable GetErrors(string? propertyName) => errorsCollection.GetErrors(propertyName);
}
