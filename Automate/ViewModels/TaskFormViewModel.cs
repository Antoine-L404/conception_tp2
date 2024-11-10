using Automate.Services.Commands;
using Automate.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class TaskFormViewModel
    {
        private Window window;

        public string EventDate { get; set; }
        public IEnumerable<EventType> EventTypes { get; set; }
        public EventType? SelectedEventType { get; set; }
        public bool IsConfirmed { get; private set; }

        public ICommand OnAddEventClick { get; }
        public ICommand OnCancelEventClick { get; }

        public TaskFormViewModel(Window openedWindow, DateTime selectedDate, EventType? initialEventType = null) 
        {
            OnAddEventClick = new RelayCommand(AddEvent);
            OnCancelEventClick = new RelayCommand(CancelEvent);

            EventDate = selectedDate.ToShortDateString();
            EventTypes = Enum.GetValues(typeof(EventType)).Cast<EventType>();

            if (initialEventType.HasValue)
                SelectedEventType = initialEventType.Value;

            window = openedWindow;
        }

        private void AddEvent()
        {
            if (SelectedEventType != null)
            {
                IsConfirmed = true;
                window.Close();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un type d'événement.", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelEvent()
        {
            IsConfirmed = false;
            window.Close();
        }
    }
}
