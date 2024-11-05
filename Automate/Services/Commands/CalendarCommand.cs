using Automate.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;
using System.Windows.Controls.Primitives;
using Automate.Utils.Constants;
using System.Linq;
using Automate.Views;

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
            new UpcomingTask { Title = EventType.Semis, EventDate = new DateTime(2024, 11, 10) },
            new UpcomingTask { Title = EventType.Rempotage, EventDate = new DateTime(2024, 11, 12) },
            new UpcomingTask { Title = EventType.Entretien, EventDate = new DateTime(2024, 11, 15) },
            new UpcomingTask { Title = EventType.Arrosage, EventDate = new DateTime(2024, 11, 20) },
            new UpcomingTask { Title = EventType.Recolte, EventDate = new DateTime(2024, 11, 30) }
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
        else if (parameter is DateTime selectedDate)
        {
            OpenEventForm(selectedDate);
        }
    }

    private void OpenEventForm(DateTime selectedDate)
    {
        TaskFormWindow eventForm = new TaskFormWindow(selectedDate);
        eventForm.ShowDialog();

        if (eventForm.IsConfirmed)
        {
            var newTask = new UpcomingTask
            {
                Title = eventForm.SelectedEventType,
                EventDate = eventForm.EventDate
            };

            AddEvent(newTask);

            MessageBox.Show($"Événement '{eventForm.SelectedEventType}' ajouté pour le {eventForm.EventDate.ToShortDateString()}");
        }
        else
        {
            MessageBox.Show("Veuillez sélectionner une date dans le calendrier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    public void AddEvent(UpcomingTask newTask)
    {
        tasks.Add(newTask);
        HighlightEventDates(); 
        ShowTaskDetails(newTask.EventDate); 
    }

    public void DeleteEvent(DateTime date)
    {
        var taskToDelete = tasks.FirstOrDefault(t => t.EventDate.Date == date.Date);

        if (taskToDelete != null)
        {
            tasks.Remove(taskToDelete); 
            HighlightEventDates(); 
            ClearTaskDetails(); 
        }
    }


    private void ClearTaskDetails()
    {
        EventTitle.Text = "Aucun événement";
        EventDate.Text = "";
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

    public void ShowTaskDetails(DateTime selectedDate)
    {
        var upcomingTask = tasks.Find(task => task.EventDate.Date == selectedDate.Date);
        if (upcomingTask != null)
        {
            EventTitle.Text = upcomingTask.Title.ToString();
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

    public UpcomingTask GetEventForDate(DateTime date)
    {
        return tasks.FirstOrDefault(task => task.EventDate.Date == date.Date);
    }


    public event EventHandler CanExecuteChanged;
}
