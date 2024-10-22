using MongoDB.Driver;

namespace Automate.Services
{
    public class MongoDBServices
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoDBServices(string databaseName)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            mongoDatabase = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return mongoDatabase.GetCollection<T>(collectionName);
        }
    }
}
