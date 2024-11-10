﻿using Automate.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;
using System.Windows.Controls.Primitives;
using Automate.Utils.Constants;
using System.Linq;
using System.Collections.ObjectModel;
using Automate.Views;
using Automate.Services;
using MongoDB.Driver;

public class CalendarCommand : ICommand
{
    private readonly List<UpcomingTask> tasks;
    public Calendar Calendar { get; set; }
    public string SelectedEventTitle { get; set; }
    public DateTime? SelectedDate { get; set; }
    public ObservableCollection<string> EventTitles { get; set; } = new ObservableCollection<string>();
    private readonly TaskCRUDService taskService;

    public CalendarCommand(TaskCRUDService taskService)
    {
        this.taskService = taskService;
        tasks = new List<UpcomingTask>();
        EventTitles.Add("Aucun événement");

        LoadTasksFromDatabase();
    }

    private void LoadTasksFromDatabase()
    {
        var taskList = taskService.GetAllTasks();
        tasks.Clear();
        tasks.AddRange(taskList);

        if (tasks.Any())
        {
            HighlightEventDates();
        }
        else
        {
            EventTitles.Add("Aucun événement");
        }
    }

    public bool CanExecute(object parameter) => SelectedDate.HasValue && !string.IsNullOrEmpty(SelectedEventTitle);

    public void Execute(object parameter)
    {
        if (parameter is Calendar myCalendar)
        {
            Calendar = myCalendar;
            HighlightEventDates();
        }
        else if (parameter is CalendarAction action)
        {
            switch (action.ActionType)
            {
                case CalendarActionType.MonthChanged:
                    HighlightEventDates();
                    break;
                case CalendarActionType.Click:
                    ShowTaskDetails(action.Date);
                    break;
                case CalendarActionType.Edit:
                    EditEvent(action);
                    break;
                case CalendarActionType.Delete:
                    DeleteEvent(action.Date);
                    break;
                default:
                    OpenEventForm(action);
                    break;
            }
        }
    }

    private void OpenEventForm(CalendarAction action)
    {
        var eventForm = new TaskFormWindow(action.Date);
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
        taskService.CreateTask(newTask);
        tasks.Add(newTask);
        HighlightEventDates();
        ShowTaskDetails(newTask.EventDate);
    }

    private void EditEvent(CalendarAction action)
    {
        if (action.Date == null || string.IsNullOrEmpty(action.Title))
        {
            MessageBox.Show("Veuillez sélectionner une date et un événement dans la liste.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var existingTask = tasks.FirstOrDefault(task => task.EventDate.Date == action.Date && task.Title.ToString() == action.Title);

        if (existingTask != null)
        {
            var eventForm = new TaskFormWindow(action.Date, existingTask.Title);
            eventForm.ShowDialog();

            if (eventForm.IsConfirmed)
            {
                existingTask.Title = eventForm.SelectedEventType;
                existingTask.EventDate = eventForm.EventDate;

                // Mise à jour dans MongoDB
                var updates = Builders<UpcomingTask>.Update
                    .Set(t => t.Title, eventForm.SelectedEventType)
                    .Set(t => t.EventDate, eventForm.EventDate);
                taskService.UpdateTask(existingTask.Id, updates);

                MessageBox.Show($"Événement '{eventForm.SelectedEventType}' modifié pour le {eventForm.EventDate.ToShortDateString()}");

                HighlightEventDates();
                ShowTaskDetails(eventForm.EventDate);
            }
        }
        else
        {
            MessageBox.Show("Aucun événement à modifier pour cette date.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }


    public void DeleteEvent(DateTime date)
    {
        var result = MessageBox.Show($"Voulez-vous vraiment supprimer l'événement du {date.ToShortDateString()} ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            var taskToDelete = tasks.FirstOrDefault(t => t.EventDate.Date == date.Date);

            if (taskToDelete != null)
            {
                taskService.DeleteTask(taskToDelete.Id); // Supprime la tâche de MongoDB
                tasks.Remove(taskToDelete);              // Supprime localement
                HighlightEventDates();
                ClearTaskDetails();
                MessageBox.Show("Événement supprimé avec succès.");
            }
            else
            {
                MessageBox.Show("Aucun événement trouvé à cette date.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }


    private void ClearTaskDetails()
    {
        EventTitles.Clear();
        EventTitles.Add("Aucun événement");
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

    public void ShowTaskDetails(DateTime selectedDate)
    {
        EventTitles.Clear();
        var eventsForDate = tasks.Where(task => task.EventDate.Date == selectedDate.Date).ToList();

        if (eventsForDate.Any())
        {
            foreach (var task in eventsForDate)
            {
                EventTitles.Add(task.Title.ToString());
            }
        }
        else
        {
            EventTitles.Add("Aucun événement");
        }
    }

    // Faite avec ai, mais accepter par Laurent
    private static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        var results = new List<T>();

        if (depObj != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T tChild) results.Add(tChild);
                results.AddRange(FindVisualChildren<T>(child));
            }
        }

        return results;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
