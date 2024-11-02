using System;
using System.Windows;
using System.Windows.Input;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private readonly CalendarCommand _calendarCommand;
        private DateTime? selectedDate;
        public CalendarWindow()
        {
            InitializeComponent();
            _calendarCommand = new CalendarCommand
            {
                Calendar = myCalendar,
                EventTitle = eventTitle,
                EventDate = eventDate
            };
        }

        private void CalendarLoaded(object sender, RoutedEventArgs e)
        {
            _calendarCommand.Execute(myCalendar);
        }

        private void OnCalendarDateClicked(object sender, MouseButtonEventArgs e)
        {
            if (myCalendar.SelectedDate.HasValue)
            {
                selectedDate = myCalendar.SelectedDate.Value;
            }
        }

        private void OnAddEventClick(object sender, RoutedEventArgs e)
        {
            if (selectedDate.HasValue)
            {
                TaskFormWindow eventForm = new TaskFormWindow(selectedDate.Value);
                eventForm.ShowDialog();

                if (eventForm.IsConfirmed)
                {
                    MessageBox.Show($"Événement '{eventForm.SelectedEventType}' ajouté pour le {eventForm.EventDate.ToShortDateString()}");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnEditEventClick(object sender, RoutedEventArgs e)
        {
            if (selectedDate.HasValue)
            {
                TaskFormWindow eventForm = new TaskFormWindow(selectedDate.Value);
                eventForm.ShowDialog();

                if (eventForm.IsConfirmed)
                {
                    MessageBox.Show($"Événement '{eventForm.SelectedEventType}' modifié pour le {eventForm.EventDate.ToShortDateString()}");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
