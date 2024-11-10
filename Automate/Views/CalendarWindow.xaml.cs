using System.Windows;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        public CalendarWindow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel(myCalendar);
        }
    }
}
