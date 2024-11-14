using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Services.Commands;
using Automate.Utils;
using Automate.Views;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class HomeViewModel
    {
        private readonly INavigationUtils navigationUtils;
        private readonly ITasksServices tasksServices;
        private Window window;

        private string criticalTaskMessage = "";
        public string CriticalTaskMessage
        {
            get
            {
                if (tasksServices.DoesTodayHasCriticalTask())
                    return "ATTENTION - Il y a un événement critique prévu aujourd'hui.";

                return "";
            }
            set
            {
                criticalTaskMessage = value;
            }
        }

        public ICommand GoToCalendarCommand { get; }
        public ICommand SignOutCommand { get; }

        public HomeViewModel(Window openedWindow, INavigationUtils navigationUtils, ITasksServices tasksServices)
        {
            window = openedWindow;
            this.navigationUtils = navigationUtils;
            this.tasksServices = tasksServices;

            GoToCalendarCommand = new RelayCommand(GoToCalendar);
            SignOutCommand = new RelayCommand(SignOut);
        }

        public void GoToCalendar()
        {
            navigationUtils.NavigateToAndCloseCurrentWindow<CalendarWindow>(window);
        }

        public void SignOut()
        {
            Environment.authenticatedUser = null;
            navigationUtils.NavigateToAndCloseCurrentWindow<LoginWindow>(window);
        }
    }
}
