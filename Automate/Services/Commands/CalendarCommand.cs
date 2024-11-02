using Automate.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;
using System.Windows.Controls.Primitives;

public class CalendarCommand : ICommand
{
    private readonly List<UpcomingTask> tasks;

    public Calendar Calendar { get; set; }
    public TextBlock EventTitle { get; set; }
    public TextBlock EventDate { get; set; }

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

    public void AddEvent(UpcomingTask newTask)
    {
        tasks.Add(newTask);
        HighlightEventDates(); // Rafraîchir pour afficher les nouvelles dates en rouge
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
            ShowTaskDetails(selectedDate);
            e.Handled = true;
        }
    }

    private void ShowTaskDetails(DateTime selectedDate)
    {
        var upcomingTask = tasks.Find(task => task.EventDate.Date == selectedDate.Date);
        if (upcomingTask != null)
        {
            EventTitle.Text = upcomingTask.Title;
            EventDate.Text = $"Date sélectionnée : {selectedDate.ToShortDateString()}";
        }
        else
        {
            EventTitle.Text = "Aucun événement";
            EventDate.Text = "";
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
