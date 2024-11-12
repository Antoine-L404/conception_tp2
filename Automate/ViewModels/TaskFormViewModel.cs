using Automate.Services.Commands;
using Automate.Utils.Enums;
using Automate.Utils.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class TaskFormViewModel: INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Window window;
        private ErrorsCollection errorsCollection;

        public string EventDate { get; set; }
        public IEnumerable<EventType> EventTypes { get; set; }

        private EventType? selectedEventType;
        public EventType? SelectedEventType 
        { 
            get => selectedEventType; 
            set
            {
                selectedEventType = value;

                if (value != null)
                {
                    errorsCollection.RemoveError(nameof(SelectedEventType));
                    NotifyErrorChange();
                }
            }
        }
        public bool IsConfirmed { get; private set; }

        public ICommand OnAddEventClick { get; }
        public ICommand OnCancelEventClick { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => errorsCollection.ContainsAnyError();
        public bool HasPasswordErrors => errorsCollection.ContainsError(nameof(SelectedEventType));
        public string ErrorMessages
        {
            get => errorsCollection.GetAllErrorMessages();
        }

        public TaskFormViewModel(Window openedWindow, DateTime selectedDate, EventType? initialEventType = null) 
        {
            OnAddEventClick = new RelayCommand(AddEvent);
            OnCancelEventClick = new RelayCommand(CancelEvent);

            errorsCollection = new ErrorsCollection(ErrorsChanged);

            EventDate = selectedDate.ToShortDateString();
            EventTypes = Enum.GetValues(typeof(EventType)).Cast<EventType>();

            if (initialEventType.HasValue)
                SelectedEventType = initialEventType.Value;

            window = openedWindow;
        }

        public IEnumerable GetErrors(string? propertyName) => errorsCollection.GetErrors(propertyName);

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                errorsCollection.AddError(nameof(SelectedEventType), "Veuillez sélectionner un type d'événement");
                NotifyErrorChange();
            }
        }

        private void CancelEvent()
        {
            IsConfirmed = false;
            window.Close();
        }

        private void NotifyErrorChange()
        {
            OnPropertyChanged(nameof(ErrorMessages));
            OnPropertyChanged(nameof(HasPasswordErrors));
        }
    }
}
