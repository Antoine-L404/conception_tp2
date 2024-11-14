using Automate.Abstract.Utils;
using Automate.Abstract.ViewModels;
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
    public class TaskFormViewModel : INotifyPropertyChanged, INotifyDataErrorInfo, ITaskFormViewModel
    {
        private Window window;
        private ErrorsCollection errorsCollection;
        private readonly INavigationUtils navigationUtils;

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
                    OnPropertyChanged(nameof(ErrorMessages));
                }
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => errorsCollection.ContainsAnyError();
        public string ErrorMessages
        {
            get => errorsCollection.GetAllErrorMessages();
        }

        public TaskFormViewModel(Window openedWindow, DateTime selectedDate, INavigationUtils navigationUtils, EventType? initialEventType = null)
        {
            AddTaskCommand = new RelayCommand(AddTask);
            CancelCommand = new RelayCommand(Cancel);

            this.navigationUtils = navigationUtils;

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

        public void AddTask()
        {
            if (SelectedEventType != null)
            {
                window.DialogResult = true;
                navigationUtils.Close(window);
            }
            else
            {
                errorsCollection.AddError(nameof(SelectedEventType), "Veuillez sélectionner un type d'événement");
                OnPropertyChanged(nameof(ErrorMessages));
            }
        }

        public void Cancel()
        {
            navigationUtils.Close(window);
        }
    }
}
