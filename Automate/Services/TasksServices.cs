using Automate.Models;
using Automate.Utils.Constants;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automate.Services
{
    public class TasksServices
    {
        private readonly IMongoCollection<UpcomingTask> tasks;

        public TasksServices(MongoDBServices mongoDBService)
        {
            tasks = mongoDBService.GetCollection<UpcomingTask>(DBConstants.TASKS_COLLECTION_NAME);
        }

        public List<UpcomingTask> GetTasksByDate(DateTime date)
        {
            return tasks.Find(task => task.EventDate == date).ToList();
        }

        public List<UpcomingTask> GetAllTasks()
        {
            return tasks.Find(new BsonDocument()).ToList();
        }

        public void CreateTask(UpcomingTask newTask)
        {
            tasks.InsertOne(newTask);
        }

        public bool UpdateTask(ObjectId taskId, UpdateDefinition<UpcomingTask> updates)
        {
            var result = tasks.UpdateOne(task => task.Id == taskId, updates);
            return result.ModifiedCount > 0;
        }

        public bool DeleteTask(ObjectId taskId)
        {
            var result = tasks.DeleteOne(task => task.Id == taskId);
            return result.DeletedCount > 0;
        }
    }
}
