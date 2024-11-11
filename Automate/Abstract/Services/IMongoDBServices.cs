using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace Automate.Abstract.Services
{
    public interface IMongoDBServices
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
        T GetOne<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> predicate);
    }
}
