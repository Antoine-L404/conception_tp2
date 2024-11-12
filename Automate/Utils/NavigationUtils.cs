using System;
using System.Windows;
using Automate.Abstract.Utils;
using Automate.Utils.Enums;
using Automate.ViewModels;
using Automate.Views;

namespace Automate.Utils
{
    public class NavigationUtils : INavigationUtils
    {
        public void NavigateTo<T>() where T : Window, new()
        {
            var window = new T();
            window.Show();
        }

        public void NavigateToAndCloseCurrentWindow<T>(Window currentWindow) where T : Window, new()
        {
            NavigateTo<T>();
            Close(currentWindow);
        }

        public void Close(Window window)
        {
            window.Close();
        }

        public TaskFormViewModel? GetTaskFormValues(DateTime taskDate, EventType? initialEventType = null)
        {
            var taskForm = new TaskFormWindow(taskDate, initialEventType);
            bool? result = taskForm.ShowDialog();

            if (result != true)
                return null;

            return taskForm.taskFormViewModel;
        }
    }
}
