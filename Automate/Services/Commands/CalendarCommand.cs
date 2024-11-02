using Automate.Models;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;

public class CalendarCommand : ICommand
{
    private readonly List<UpcomingTask> tasks;

    public Calendar Calendar { get; set; }
    public Popup Popup { get; set; }
    public TextBlock PopupTitle { get; set; }
    public TextBlock PopupText { get; set; }

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

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        if (parameter is Calendar)
        {
            HighlightEventDates();
        }
        else if (parameter is MouseButtonEventArgs e)
        {
            ClickOnDate(e);
        }
    }

    private void HighlightEventDates()
    {
        if (Calendar == null) return;

        foreach (var calendarDayButton in FindVisualChildren<CalendarDayButton>(Calendar))
        {
            if (calendarDayButton.DataContext is DateTime date)
            {
                var isEvent = tasks.Exists(t => t.EventDate.Date == date.Date);
                calendarDayButton.Background = isEvent ? new SolidColorBrush(Colors.LightCoral) : new SolidColorBrush(Colors.Transparent);
            }
        }
    }

    private void ClickOnDate(MouseButtonEventArgs e)
    {
        var calendarDayButton = FindParent<CalendarDayButton>(e.OriginalSource as DependencyObject);

        if (calendarDayButton != null && calendarDayButton.DataContext is DateTime selectedDate)
        {
            ShowTaskPopup(selectedDate);
            e.Handled = true;
        }
    }

    private void ShowTaskPopup(DateTime selectedDate)
    {
        if (Popup == null || PopupTitle == null || PopupText == null) return;

        var upcomingTask = tasks.Find(task => task.EventDate.Date == selectedDate.Date);
        if (upcomingTask != null)
        {
            Popup.IsOpen = true;
            PopupTitle.Text = upcomingTask.Title;
            PopupText.Text = $"Date sélectionnée : {selectedDate.ToShortDateString()}";
        }
        else
        {
            Popup.IsOpen = false;
        }
    }

    private static T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject parentObject = VisualTreeHelper.GetParent(child);
        if (parentObject == null)
            return null;
        if (parentObject is T parent)
            return parent;
        return FindParent<T>(parentObject);
    }

    private static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        var results = new List<T>();

        if (depObj != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T tChild)
                {
                    results.Add(tChild);
                }

                results.AddRange(FindVisualChildren<T>(child));
            }
        }

        return results;
    }


    public event EventHandler CanExecuteChanged;
}
