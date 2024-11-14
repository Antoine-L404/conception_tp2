using Automate.Abstract.Utils;
using Automate.Services.Commands;
using Automate.Views;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class HomeViewModel
    {
        private readonly INavigationUtils navigationUtils;
        private Window window;

        public ICommand GoToCalendarCommand { get; }

        public HomeViewModel(Window openedWindow, INavigationUtils navigationUtils)
        {
            window = openedWindow;
            this.navigationUtils = navigationUtils;
            GoToCalendarCommand = new RelayCommand(GoToCalendar);
        }

        public void GoToCalendar()
        {
            navigationUtils.NavigateToAndCloseCurrentWindow<CalendarWindow>(window);
        }
    }
}
