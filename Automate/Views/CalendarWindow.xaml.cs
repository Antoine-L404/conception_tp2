using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Automate.Models;
using Automate.ViewModels;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private DateTime? selectedDate;
        private readonly CalendarCommand _calendarCommand;

        public CalendarWindow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel(myCalendar, eventTitle, eventDate);
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

            selectedDate = DateTime.Today;
            _calendarCommand.ShowTaskDetails(selectedDate.Value);
        }

    }
}
