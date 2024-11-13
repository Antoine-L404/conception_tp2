using Automate.Services.Commands;
using Automate.Utils.Constants;
using System.Windows.Input;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Automate.Models;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Environment = Automate.Utils.Environment;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Automate.Utils.Enums;
using Automate.Utils.Validation;
using System.Collections;
using MongoDB.Driver;
using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.ViewModels;
using Automate.Abstract.ViewModels;

public class CalendarViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly string selectDateErrorMessage = "Veuillez sélectionner une date dans le calendrier.";
    private readonly string selectEventTitleErrorMessage = "Veuillez sélectionner un événement à modifier ou supprimer.";
    private readonly string noEvenTitle = "Aucun événement";

    private readonly ITasksServices tasksServices;
    private readonly INavigationUtils navigationUtils;

    private ErrorsCollection errorsCollection;
    private List<UpcomingTask> selectedDateTasks;

    public ICommand AddTaskCommand { get; }
    public ICommand EditTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand MonthChangedCommand { get; }
    public ICommand DateSelectedCommand { get; }

    public bool HasErrors => errorsCollection.ContainsAnyError();
    public string ErrorMessages
    {
        get => errorsCollection.GetAllErrorMessages();
    }
    public string? SuccessMessage { get; set; }

    public DateTime? SelectedDate { get; set; }
    public string? SelectedEventTitle { get; set; }
    public Calendar Calendar { get; set; }
    public ObservableCollection<string> EventTitles { get; set; } = new ObservableCollection<string>();

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool IsAdmin 
    {
        get => Environment.authenticatedUser.Role == RoleConstants.ADMIN;
    }

    public CalendarViewModel(Calendar calendar, ITasksServices tasksServices, INavigationUtils navigationUtils)
    {
        Calendar = calendar;
        this.tasksServices = tasksServices;
        this.navigationUtils = navigationUtils;

        errorsCollection = new ErrorsCollection(ErrorsChanged);
        selectedDateTasks = new List<UpcomingTask>();

        AddTaskCommand = new RelayCommand(AddTask);
        EditTaskCommand = new RelayCommand(EditTask);
        DeleteTaskCommand = new RelayCommand(DeleteTask);
        DateSelectedCommand = new RelayCommand(DateSelected);
        MonthChangedCommand = new RelayCommand(HighlightEventDates);
        
        HighlightEventDates();
        ShowTaskDetails(DateTime.Today);
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void NotifyErrorChange()
    {
        OnPropertyChanged(nameof(ErrorMessages));

        SuccessMessage = string.Empty;
        OnPropertyChanged(nameof(SuccessMessage));
    }

    private void NotifySuccessMessageChange(string message)
    {
        SuccessMessage = message;
        OnPropertyChanged(nameof(SuccessMessage));
    }

    public IEnumerable GetErrors(string? propertyName) => errorsCollection.GetErrors(propertyName);

    public void DateSelected()
    {
        if (ValidateSelectedDate())
            ShowTaskDetails(SelectedDate!.Value);
    }

    public void AddTask()
    {
        if (!ValidateSelectedDate())
            return;

        HandleAddTaskForm(SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    public void EditTask()
    {
        if (!ValidateSelectedDate() || 
            !ValidateSelectedEventTitle() || 
            !ValidateExistingTask(SelectedEventTitle!))
            return;

        HandleEditForm(SelectedEventTitle!, SelectedDate!.Value);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    public void DeleteTask()
    {
        if (!ValidateSelectedDate() ||
            !ValidateSelectedEventTitle() ||
            !ValidateExistingTask(SelectedEventTitle!))
            return;

        HandleDelete(SelectedEventTitle!);
        HighlightEventDates();
        ShowTaskDetails(SelectedDate!.Value);
    }

    private void ShowTaskDetails(DateTime selectedDate)
    {
        EventTitles.Clear();
        List<UpcomingTask> tasks = tasksServices.GetTasksByDate(selectedDate);

        if (tasks.Count > 0)
        {
            foreach (var task in tasks)
            {
                EventTitles.Add(task.Title.ToString());
                selectedDateTasks.Add(task);
            }
        }
        else
        {
            EventTitles.Add(noEvenTitle);
            selectedDateTasks.Clear();
        }
    }

    private void HighlightEventDates()
    {
        foreach (var calendarDayButton in FindVisualChildren<CalendarDayButton>(Calendar))
        {
            if (calendarDayButton.DataContext is DateTime date)
            {
                List<UpcomingTask> tasks = tasksServices.GetTasksByDate(date);
                calendarDayButton.Background = 
                    tasks.Count > 0 ? new SolidColorBrush(Colors.LightCoral) : new SolidColorBrush(Colors.Transparent);
            }
        }
    }

    // Faite avec ai, mais accepté par Laurent
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

    private void HandleAddTaskForm(DateTime taskDate)
    {
        ITaskFormViewModel? taskFormViewModel = navigationUtils.GetTaskFormValues(taskDate);
        if (taskFormViewModel == null)
            return;

        var newTask = new UpcomingTask
        {
            Title = (EventType)taskFormViewModel.SelectedEventType!,
            EventDate = taskDate
        };

        tasksServices.CreateTask(newTask);

        NotifySuccessMessageChange(
            $"Événement '{taskFormViewModel.SelectedEventType}' ajouté pour le {taskDate.ToShortDateString()}");
    }

    private void HandleEditForm(string taskTitle, DateTime taskDate)
    {
        UpcomingTask taskToEdit = selectedDateTasks.Find(task => task.Title.ToString() == taskTitle)!;

        ITaskFormViewModel? taskFormViewModel = navigationUtils.GetTaskFormValues(taskDate, taskToEdit.Title);
        if (taskFormViewModel == null)
            return;

        var updateDefinition = Builders<UpcomingTask>.Update
            .Set(t => t.Title, taskFormViewModel.SelectedEventType)
            .Set(t => t.EventDate, taskDate);
        tasksServices.UpdateTask(taskToEdit.Id, updateDefinition);

        NotifySuccessMessageChange(
            $"Événement '{taskFormViewModel.SelectedEventType}' modifié pour le {taskDate.ToShortDateString()}");
    }

    public void HandleDelete(string taskTitle)
    {
        UpcomingTask taskToDelete = selectedDateTasks.Find(task => task.Title.ToString() == taskTitle)!;

        tasksServices.DeleteTask(taskToDelete.Id);

        NotifySuccessMessageChange($"Événement '{taskToDelete.Title}' supprimé avec succès");
    }

    private bool ValidateSelectedDate()
    {
        return CommonValidation.ValidateNull(
            nameof(SelectedEventTitle),
            SelectedDate,
            selectDateErrorMessage,
            errorsCollection,
            NotifyErrorChange
        );
    }

    private bool ValidateSelectedEventTitle()
    {
        return CommonValidation.ValidateNull(
            nameof(SelectedEventTitle),
            SelectedEventTitle,
            selectEventTitleErrorMessage,
            errorsCollection,
            NotifyErrorChange
        );
    }

    private bool ValidateExistingTask(string taskTitle)
    {
        UpcomingTask? existingTask = selectedDateTasks.Find(task => task.Title.ToString() == taskTitle);

        if (existingTask == null)
        {
            errorsCollection.AddError(nameof(SelectedEventTitle), "Aucun événement existant à cette date.");
            NotifyErrorChange();
        }
        else
        {
            errorsCollection.RemoveError(nameof(SelectedEventTitle));
            NotifyErrorChange();
        }
        
        return existingTask != null;
    }
}
