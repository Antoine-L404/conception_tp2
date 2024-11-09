using Automate.Services.Commands;
using Automate.Utils.Constants;
using Automate.Views;
using System.Windows.Input;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;

public class CalendarViewModel
{
    public CalendarCommand CalendarCommand { get; }
    public ICommand OnAddEventClick { get; }
    public ICommand OnEditEventClick { get; }
    public ICommand OnDeleteEventClick { get; }
    public ICommand OnMonthChanged { get; }
    public ICommand ClickOnDate {  get; }
    public Calendar Calendar { get; set; }
    public ObservableCollection<string> EventTitles { get; set; } = new ObservableCollection<string>();

    private DateTime? selectedDate;
    public DateTime? SelectedDate
    {
        get => selectedDate;
        set
        {
            selectedDate = value;
        }
    }

    public CalendarViewModel(Calendar myCalendar, ObservableCollection<string> eventTitles)
    {
        this.Calendar = myCalendar;
        this.EventTitles = eventTitles;

        CalendarCommand = new CalendarCommand()
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
        if (selectedDate != null)
        {
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Click, selectedDate.Value));
        }
        else
        {
            MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void AddEvent()
    {
        if (selectedDate is not null)
        {
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Add, selectedDate.Value));
        }
        else
        {
            MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void EditEvent()
    {
        if (selectedDate is not null)
        {
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Edit, selectedDate.Value));
        }
        else
        {
            MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void DeleteEvent()
    {
        if (selectedDate is not null)
        {
            CalendarCommand.Execute(new CalendarAction(CalendarActionType.Delete, selectedDate.Value));
        }
        else
        {
            MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
