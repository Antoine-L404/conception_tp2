using Automate.Services.Commands;
using Automate.Utils.Constants;
using Automate.Views;
using System.Windows.Input;
using System.Windows;
using System;
using System.Windows.Controls;

public class CalendarViewModel
{
    public CalendarCommand CalendarCommand { get; }
    public ICommand OnAddEventClick { get; }
    public ICommand OnEditEventClick { get; }
    public ICommand OnDeleteEventClick { get; }
    public Calendar Calendar { get; set; }
    public TextBlock EventTitle { get; set; }
    public TextBlock EventDate { get; set; }

    private DateTime? selectedDate;
    public DateTime? SelectedDate
    {
        get => selectedDate;
        set
        {
            selectedDate = value;
        }
    }

    // Modifie le constructeur pour accepter `CalendarWindow`
    public CalendarViewModel(Calendar myCalendar, TextBlock eventTitle, TextBlock eventDate)
    {
        this.Calendar = myCalendar;
        this.EventTitle = eventTitle;
        this.EventDate = eventDate;

        CalendarCommand = new CalendarCommand()
        {
            Calendar = myCalendar,
            EventTitle = eventTitle,
            EventDate = eventDate
        };

        OnAddEventClick = new RelayCommand(AddEvent);
        OnEditEventClick = new RelayCommand(EditEvent);
        OnDeleteEventClick = new RelayCommand(DeleteEvent);
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
