using Automate.Models;
using BC = BCrypt.Net.BCrypt;
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

        public User? Authenticate(string username, string password)
        {
            User? user = users.Find(u => u.Username == username && u.Password == HashPassword(password)).FirstOrDefault();
            return user;
        }

        public string HashPassword(string password) => BC.HashPassword(password);

        public void RegisterUser(User user)
        {
            users.InsertOne(user);
        }
    }
}
