using Automate.Utils.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Automate.Abstract.ViewModels
{
    public interface ITaskFormViewModel
    {
        ICommand AddTaskCommand { get; }
        ICommand CancelCommand { get; }
        string ErrorMessages { get; }
        string EventDate { get; set; }
        IEnumerable<EventType> EventTypes { get; set; }
        bool HasErrors { get; }
        bool HasPasswordErrors { get; }
        EventType? SelectedEventType { get; set; }

        event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        event PropertyChangedEventHandler? PropertyChanged;

        void AddTask();
        void Cancel();
        IEnumerable GetErrors(string? propertyName);
    }
}