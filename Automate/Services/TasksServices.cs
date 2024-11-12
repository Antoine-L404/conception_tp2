using Automate.Abstract.Services;
using Automate.Models;
using Automate.Utils.Constants;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Automate.Services
{
    public class TasksServices
    {
        private readonly IMongoDBServices mongoDBService;
        private readonly IMongoCollection<UpcomingTask> tasks;

        public TasksServices(IMongoDBServices mongoDBService)
        {
            this.mongoDBService = mongoDBService;
            tasks = mongoDBService.GetCollection<UpcomingTask>(DBConstants.TASKS_COLLECTION_NAME);
        }

        public List<UpcomingTask> GetTasksByDate(DateTime date)
        {
            return mongoDBService.GetMany(tasks, task => task.EventDate == date);
        }

        public List<UpcomingTask> GetAllTasks()
        {
            return mongoDBService.GetAll(tasks);
        }

        public void CreateTask(UpcomingTask newTask)
        {
            mongoDBService.InsertOne(tasks, newTask);
        }

        public bool UpdateTask(ObjectId taskId, UpdateDefinition<UpcomingTask> updates)
        {
            return mongoDBService.UpdateOne(tasks, taskId, updates);
        }

        public bool DeleteTask(ObjectId taskId)
        {
            return mongoDBService.DeleteOne(tasks, taskId);
        }
    }
}
