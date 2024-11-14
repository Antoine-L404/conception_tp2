using Automate.Abstract.ViewModels;
using Automate.Utils.Enums;
using System;
using System.Windows;

namespace Automate.Abstract.Utils
{
    public interface INavigationUtils
    {
        void Close(Window window);
        void NavigateTo<T>() where T : Window, new();
        void NavigateToAndCloseCurrentWindow<T>(Window currentWindow) where T : Window, new();
        ITaskFormViewModel? GetTaskFormValues(DateTime taskDate, EventType? initialEventType = null);
    }
}