using MongoDB.Driver;

namespace Automate.Services
{
    public class MongoDBServices
    {
        private readonly IMongoDatabase _database;

        public MongoDBServices(string databaseName)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
