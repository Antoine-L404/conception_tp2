using Automate.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Automate.Services
{
    public class TaskCRUDService
    {
        private readonly IMongoCollection<UpcomingTask> tasks;

        public TaskCRUDService(MongoDBServices mongoDBService)
        {
            tasks = mongoDBService.GetCollection<UpcomingTask>("Task");
        }

        public UpcomingTask? GetTask(ObjectId taskId)
        {
            return tasks.Find(task => task.Id == taskId).FirstOrDefault();
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
