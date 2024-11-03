using System;
using System.Windows;
using System.Windows.Input;
using Automate.Models;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private DateTime? selectedDate;
        private readonly CalendarCommand _calendarCommand;

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

            selectedDate = DateTime.Today;
            _calendarCommand.ShowTaskDetails(selectedDate.Value);
        }

        private void OnCalendarDateClicked(object sender, MouseButtonEventArgs e)
        {
            if (myCalendar.SelectedDate.HasValue)
            {
                selectedDate = myCalendar.SelectedDate.Value;
                _calendarCommand.ShowTaskDetails(selectedDate.Value); 
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
                    var newTask = new UpcomingTask
                    {
                        Title = eventForm.SelectedEventType,
                        EventDate = eventForm.EventDate
                    };

                    _calendarCommand.AddEvent(newTask);
                    _calendarCommand.Execute(myCalendar); 
                    MessageBox.Show($"Événement '{eventForm.SelectedEventType}' ajouté pour le {eventForm.EventDate.ToShortDateString()}");

                    _calendarCommand.ShowTaskDetails(eventForm.EventDate);
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
                var existingTask = _calendarCommand.GetEventForDate(selectedDate.Value);
                if (existingTask != null)
                {
                    TaskFormWindow eventForm = new TaskFormWindow(selectedDate.Value, existingTask.Title);
                    eventForm.ShowDialog();

                    if (eventForm.IsConfirmed)
                    {
                        existingTask.Title = eventForm.SelectedEventType;
                        existingTask.EventDate = eventForm.EventDate;
                        MessageBox.Show($"Événement '{eventForm.SelectedEventType}' modifié pour le {eventForm.EventDate.ToShortDateString()}");

                        _calendarCommand.Execute(myCalendar);
                        _calendarCommand.ShowTaskDetails(eventForm.EventDate);
                    }
                }
                else
                {
                    MessageBox.Show("Aucun événement à modifier pour cette date.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnDeleteEventClick(object sender, RoutedEventArgs e)
        {
            if (selectedDate.HasValue)
            {
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer l'événement du {selectedDate.Value.ToShortDateString()} ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _calendarCommand.DeleteEvent(selectedDate.Value); // Supprime l'événement pour la date sélectionnée
                    MessageBox.Show("Événement supprimé avec succès.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



    }
}
