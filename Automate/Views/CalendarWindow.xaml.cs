using Automate.Utils;
using System.Windows;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        public CalendarWindow()
        {
            InitializeComponent();
        }

        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new CalendarViewModel(calendar, Environment.tasksServices, new NavigationUtils());
        }
    }
}
