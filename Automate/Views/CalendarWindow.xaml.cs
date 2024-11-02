using System;
using System.Windows;
using System.Windows.Input;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private readonly CalendarCommand _calendarCommand;

        public CalendarWindow()
        {
            InitializeComponent();
            _calendarCommand = new CalendarCommand
            {
                Calendar = myCalendar,
                EventTitle = eventTitle,
                EventDate = eventDate
            };
        }

        private void CalendarLoaded(object sender, RoutedEventArgs e)
        {
            _calendarCommand.Execute(myCalendar);
        }

        private void OnCalendarDateClicked(object sender, MouseButtonEventArgs e)
        {
            _calendarCommand.Execute(e);
        }
    }
}
