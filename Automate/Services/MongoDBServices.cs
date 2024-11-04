using MongoDB.Driver;
using Automate.Utils.Constants;
using System.Threading.Tasks;

namespace Automate.Services
{
    public class MongoDBServices
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
    }
}
