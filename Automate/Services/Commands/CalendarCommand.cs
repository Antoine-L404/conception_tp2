using Automate.Models;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Linq;
using Automate.Views;
using Automate.Services;
using MongoDB.Driver;
using Automate.Utils.Enums;

public class CalendarCommand
{
    private readonly List<UpcomingTask> tasks;

    private readonly TaskCRUDService taskService;

    public CalendarCommand(TaskCRUDService taskService)
    {
        this.taskService = taskService;
        tasks = taskService.GetAllTasks();
    }

    public void AddTask(DateTime taskDate)
    {
        var eventForm = new TaskFormWindow(taskDate);
        eventForm.ShowDialog();

        if (eventForm.taskFormViewModel.IsConfirmed)
        {
            var newTask = new UpcomingTask
            {
                Title = (EventType)eventForm.taskFormViewModel.SelectedEventType!,
                EventDate = taskDate
            };

            taskService.CreateTask(newTask);
            tasks.Add(newTask);

            MessageBox.Show(
                $"Événement '{eventForm.taskFormViewModel.SelectedEventType}' ajouté pour le {taskDate.ToShortDateString()}");
        }
    }

    public void EditTask(string taskTitle, DateTime taskDate)
    {
        var existingTask = tasks.FirstOrDefault(task => task.EventDate.Date == taskDate && task.Title.ToString() == taskTitle);

        if (existingTask != null)
        {
            HandleEditForm(existingTask, taskDate);
        }
        else
        {
            MessageBox.Show("Aucun événement à modifier pour cette date.", "Erreur", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void HandleEditForm(UpcomingTask existingTask, DateTime taskDate)
    {
        var eventForm = new TaskFormWindow(taskDate, existingTask.Title);
        eventForm.ShowDialog();

        if (eventForm.taskFormViewModel.IsConfirmed)
        {
            var updateDefinition = Builders<UpcomingTask>.Update
                .Set(t => t.Title, eventForm.taskFormViewModel.SelectedEventType)
                .Set(t => t.EventDate, taskDate);
            taskService.UpdateTask(existingTask.Id, updateDefinition);

            MessageBox.Show(
                $"Événement '{eventForm.taskFormViewModel.SelectedEventType}' modifié pour le {taskDate.ToShortDateString()}");
        }
    }

    public void DeleteTask(DateTime taskDate)
    {
        if (!AskForDeletion(taskDate))
            return;

        var taskToDelete = tasks.FirstOrDefault(t => t.EventDate.Date == taskDate.Date);

        if (taskToDelete != null)
        {
            taskService.DeleteTask(taskToDelete.Id);
            tasks.Remove(taskToDelete);

            MessageBox.Show("Événement supprimé avec succès.");
        }
        else
        {
            MessageBox.Show("Aucun événement trouvé à cette date.", "Erreur", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private bool AskForDeletion(DateTime taskDate)
    {
        var result = MessageBox.Show(
            $"Voulez-vous vraiment supprimer l'événement du {taskDate.ToShortDateString()} ?",
            "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        return result == MessageBoxResult.Yes;
    }
}

