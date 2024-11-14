using MongoDB.Driver;
using Automate.Utils.Constants;
using Automate.Abstract.Services;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using MongoDB.Bson;
using Automate.Abstract.Models;

namespace Automate.Services
{
    public class MongoDBServices: IMongoDBServices
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoDBServices(string databaseName)
        {
            MongoClient client = new MongoClient(DBConstants.DB_URL);
            mongoDatabase = client.GetDatabase(databaseName);
        }
        
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return mongoDatabase.GetCollection<T>(collectionName);
        }

        public T GetOne<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> predicate)
        {
            return collection.Find(predicate).FirstOrDefault();
        }

        public List<T> GetAll<T>(IMongoCollection<T> collection)
        {
            return collection.Find(new BsonDocument()).ToList();
        }

        public List<T> GetMany<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> predicate)
        {
            return collection.Find(predicate).ToList();
        }

        public void InsertOne<T>(IMongoCollection<T> collection, T newItem)
        {
            collection.InsertOne(newItem);
        }

        public bool UpdateOne<T>(IMongoCollection<T> collection, ObjectId objectId, UpdateDefinition<T> updates) 
            where T : IObjectWithId
        {
            var result = collection.UpdateOne(obj => obj.Id == objectId, updates);
            return result.ModifiedCount > 0;
        }

        public bool DeleteOne<T>(IMongoCollection<T> collection, ObjectId objectId) where T : IObjectWithId
        {
            var result = collection.DeleteOne(obj => obj.Id == objectId);
            return result.DeletedCount > 0;
        }
    }
}
