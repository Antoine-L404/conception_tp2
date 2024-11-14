using Automate.Utils;
using Automate.Utils.Enums;
using Automate.ViewModels;
using System;
using System.Windows;

namespace Automate.Views
{
    public partial class TaskFormWindow : Window
    {
        public TaskFormViewModel taskFormViewModel { get; private set; }

        public TaskFormWindow(DateTime selectedDate, EventType? initialEventType = null)
        {
            InitializeComponent();
            taskFormViewModel = new TaskFormViewModel(this, selectedDate, new NavigationUtils(), initialEventType);
            DataContext = taskFormViewModel;
        }
    }
}
