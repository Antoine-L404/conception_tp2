using Automate.Abstract.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Automate.Abstract.Services
{
    public interface IMongoDBServices
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
        T GetOne<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> predicate);
        List<T> GetAll<T>(IMongoCollection<T> collection);
        List<T> GetMany<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> predicate);
        void InsertOne<T>(IMongoCollection<T> collection, T newItem);
        bool UpdateOne<T>(IMongoCollection<T> collection, ObjectId objectId, UpdateDefinition<T> updates) where T : IObjectWithId;
        bool DeleteOne<T>(IMongoCollection<T> collection, ObjectId objectId) where T : IObjectWithId;
    }
}
