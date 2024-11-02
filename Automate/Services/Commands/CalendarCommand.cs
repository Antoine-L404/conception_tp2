using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Automate.Models;
using Automate.Services.Commands;
using System.Windows.Input;
using System.Windows;

namespace Automate.Commands
{
    public class CalendarCommand : ICommand
    {
        private readonly List<UpcomingTask> tasks;

        public CalendarCommand()
        {
            tasks = new List<UpcomingTask>
            {
                new UpcomingTask { Title = "Event 1", EventDate = new DateTime(2024, 11, 10) },
                new UpcomingTask { Title = "Event 2", EventDate = new DateTime(2024, 11, 12) },
                new UpcomingTask { Title = "Event 3", EventDate = new DateTime(2024, 11, 15) },
                new UpcomingTask { Title = "Event 4", EventDate = new DateTime(2024, 11, 20) },
                new UpcomingTask { Title = "Event 5", EventDate = new DateTime(2024, 11, 30) }
            };
        }

        public bool CanExecute(object parameter)
        {
            return parameter is Calendar || parameter is Tuple<Calendar, Popup, TextBlock, TextBlock, DateTime>;
        }

        public void Execute(object parameter)
        {
            if (parameter is Calendar calendar)
            {
                HighlightEventDates(calendar);
            }
            else if (parameter is Tuple<Calendar, Popup, TextBlock, TextBlock, DateTime> context)
            {
                ShowTaskPopup(context.Item1, context.Item2, context.Item3, context.Item4, context.Item5);
            }
        }

        private void HighlightEventDates(Calendar calendar)
        {
            foreach (var calendarDayButton in FindVisualChildren<CalendarDayButton>(calendar))
            {
                if (calendarDayButton.DataContext is DateTime date)
                {
                    var isEvent = tasks.Exists(t => t.EventDate.Date == date.Date);
                    calendarDayButton.Background = isEvent ? new SolidColorBrush(Colors.LightCoral) : new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        private void ShowTaskPopup(Calendar calendar, Popup popup, TextBlock popupTitle, TextBlock popupText, DateTime selectedDate)
        {
            var upcomingTask = tasks.Find(task => task.EventDate.Date == selectedDate.Date);
            if (upcomingTask != null)
            {
                popup.IsOpen = true;
                popupTitle.Text = upcomingTask.Title;
                popupText.Text = $"Date sélectionnée : {selectedDate.ToShortDateString()}";
            }
            else
            {
                popup.IsOpen = false;
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T tChild)
                    {
                        yield return tChild;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
