using System;
using System.Collections.ObjectModel;
using System.Windows;
using Automate.Services;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private DateTime? selectedDate;
        private readonly CalendarCommand calendarCommand;
        private ObservableCollection<string> eventTitle = new ObservableCollection<string>();

        public CalendarWindow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel(myCalendar, eventTitle);
            var mongoDBService = new MongoDBServices("Automate");
            var taskService = new TaskCRUDService(mongoDBService);
            calendarCommand = new CalendarCommand(taskService)
            {
                Calendar = myCalendar,
                EventTitles = eventTitle,
            };
        }

        private void CalendarLoaded(object sender, RoutedEventArgs e)
        {
            calendarCommand.Execute(myCalendar);

            selectedDate = DateTime.Today;
            calendarCommand.ShowTaskDetails(selectedDate.Value);
        }

    }
}
