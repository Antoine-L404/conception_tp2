using MongoDB.Driver;
using Automate.Utils.Constants;
using Automate.Abstract.Services;
using System;
using System.Linq.Expressions;

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
    }
}
