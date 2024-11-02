using System;
using System.Windows;
using System.Windows.Input;
using Automate.Services.Commands;

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
                Popup = myPopup,
                PopupTitle = myPopupTitle,
                PopupText = myPopupText
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
