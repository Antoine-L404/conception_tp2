using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private List<CalendarEvent> events;

        public CalendarWindow()
        {
            InitializeComponent();
            InitializeEvents();
            HighlightEventDates();
        }

        private void InitializeEvents()
        {
            events = new List<CalendarEvent>
            {
                new CalendarEvent { Title = "Event 1", Date = new DateTime(2024, 11, 10) },
                new CalendarEvent { Title = "Event 2", Date = new DateTime(2024, 11, 12) },
                new CalendarEvent { Title = "Event 3", Date = new DateTime(2024, 11, 15) },
                new CalendarEvent { Title = "Event 4", Date = new DateTime(2024, 11, 20) },
                new CalendarEvent { Title = "Event 5", Date = new DateTime(2024, 11, 30) }
            };
        }

        private void HighlightEventDates()
        {
            foreach (var calendarEvent in events)
            {
                var date = calendarEvent.Date;

                // Vérifiez si la date est dans la plage d'affichage du calendrier
                if (date >= myCalendar.DisplayDate.AddDays(-myCalendar.DisplayDate.Day) &&
                    date <= myCalendar.DisplayDate.AddMonths(1))
                {
                    // Changez la date sélectionnée pour la date d'événement
                    if (myCalendar.SelectedDate == null)
                    {
                        myCalendar.SelectedDate = date;
                    }
                }
            }
        }

        private void MyCalendar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is CalendarDayButton calendarDayButton)
            {
                DateTime selectedDate = (DateTime)calendarDayButton.Content;

                // Montrez le Popup avec le titre de l'événement si la date est un événement
                var calendarEvent = events.Find(e => e.Date.Date == selectedDate.Date);
                if (calendarEvent != null)
                {
                    myPopup.IsOpen = true;
                    myPopupTitle.Text = calendarEvent.Title;
                    myPopupText.Text = $"Date sélectionnée : {selectedDate.ToShortDateString()}";
                }
                else
                {
                    myPopup.IsOpen = false; // Fermer le popup si aucune date d'événement
                }
            }
        }
    }

    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}
