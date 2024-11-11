using Automate.Services.Commands;
using Automate.Utils;
using Automate.Views;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class HomeViewModel
    {
        private readonly NavigationUtils navigationUtils;
        private Window window;
        public ICommand GoToCalendarCommand { get; }

        public HomeViewModel(Window openedWindow)
        {
            this.navigationUtils = new NavigationUtils();
            this.window = openedWindow;
            this.GoToCalendarCommand = new RelayCommand(GoToCalendar);
        }

        public void GoToCalendar()
        {
            navigationUtils.NavigateToAndCloseCurrentWindow<HomeWindow>(window); //should navigate to calendar, change when it's merged
        }
    }
}
