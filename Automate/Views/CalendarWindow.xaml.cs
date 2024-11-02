using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Automate.Models;

namespace Automate.Views
{
    public partial class CalendarWindow : Window
    {
        private List<UpcomingTask> tasks;

        public CalendarWindow()
        {
            InitializeComponent();
            InitializeTasks();
            HighlightEventDates();
        }

        private void InitializeTasks()
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

        private void MyCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            HighlightEventDates();
        }

        private void HighlightEventDates()
        {
            foreach (var calendarDayButton in FindVisualChildren<CalendarDayButton>(myCalendar))
            {
                if (calendarDayButton.DataContext is DateTime date)
                {
                    var isEvent = tasks.Exists(t => t.EventDate.Date == date.Date);
                    if (isEvent)
                    {
                        calendarDayButton.Background = new SolidColorBrush(Colors.LightCoral);
                    }
                    else
                    {
                        calendarDayButton.Background = new SolidColorBrush(Colors.Transparent);
                    }
                }
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

        private void MyCalendar_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var calendarDayButton = FindParent<CalendarDayButton>(e.OriginalSource as DependencyObject);

            if (calendarDayButton != null && calendarDayButton.DataContext is DateTime selectedDate)
            {
                var upcomingTask = tasks.Find(task => task.EventDate.Date == selectedDate.Date);
                if (upcomingTask != null)
                {
                    myPopup.IsOpen = true;
                    myPopupTitle.Text = upcomingTask.Title;
                    myPopupText.Text = $"Date sélectionnée : {selectedDate.ToShortDateString()}";
                }
                else
                {
                    myPopup.IsOpen = false; 
                }

                e.Handled = true; 
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            if (parentObject is T parent)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }
    }
}
