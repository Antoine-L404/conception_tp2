using Automate.Services.Commands;
using Automate.Utils.Constants;
using System.Windows.Input;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Automate.Services;
using Automate.Utils.Enums;

public class CalendarViewModel
{
    private readonly string selectDateErrorMessage = "Veuillez sélectionner une date dans le calendrier.";
    private readonly string selectEventTitleErrorMessage = "Veuillez sélectionner un événement à modifier.";
    private readonly string errorTitle = "Erreur";

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

    public CalendarViewModel(Calendar myCalendar, ObservableCollection<string> eventTitles)
    {
        Calendar = myCalendar;
        EventTitles = eventTitles;

        var mongoDBService = new MongoDBServices(DBConstants.DB_NAME);
        var taskService = new TaskCRUDService(mongoDBService);
        CalendarCommand = new CalendarCommand(taskService)
        {
            Calendar = myCalendar,
            EventTitles = eventTitles,
        };

        OnAddEventClick = new RelayCommand(AddEvent);
        OnEditEventClick = new RelayCommand(EditEvent);
        OnDeleteEventClick = new RelayCommand(DeleteEvent);
        ClickOnDate = new RelayCommand(ClickEvent);
        OnMonthChanged = new RelayCommand(MonthChanged);
    }


    private void MonthChanged()
    {
        CalendarCommand.Execute(new CalendarAction(CalendarActionType.MonthChanged));
    }

    private void ClickEvent()
    {
        if (ValidateSelectedDate())
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Click, SelectedDate!.Value));
    }

    private void AddEvent()
    {
        if (ValidateSelectedDate())
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Add, SelectedDate!.Value));
    }

    private void EditEvent()
    {
        if (ValidateSelectedDate() && ValidateSelectedEventTitle())
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Edit, SelectedDate!.Value, SelectedEventTitle!));
    }

    private void DeleteEvent()
    {
        if (ValidateSelectedDate())
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Delete, SelectedDate!.Value));
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
