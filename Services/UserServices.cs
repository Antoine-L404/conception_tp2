using Automate.Models;
using MongoDB.Driver;
using System.Linq;

namespace Automate.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> users;

        public UserServices(MongoDBServices mongoDBService)
        {
            users = mongoDBService.GetCollection<User>("Users");
        }

        public User Authenticate(string? username, string? password)
        {
            var user = users.Find(u => u.Username == username && u.Password == password).FirstOrDefault();
            return user;
        }

        public void RegisterUser(User user)
        {
            users.InsertOne(user);
        }
    }
}
