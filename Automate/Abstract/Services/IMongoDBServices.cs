using MongoDB.Driver;

namespace Automate.Abstract.Services
{
    public interface IMongoDBServices
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
