using Automate.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Automate.Abstract.Services
{
    public interface ITasksServices
    {
        void CreateTask(UpcomingTask newTask);
        bool DeleteTask(ObjectId taskId);
        List<UpcomingTask> GetAllTasks();
        List<UpcomingTask> GetTasksByDate(DateTime date);
        bool UpdateTask(ObjectId taskId, UpdateDefinition<UpcomingTask> updates);
    }
}