using Automate.Models;
using BC = BCrypt.Net.BCrypt;
using MongoDB.Driver;
using System.Linq;
using Automate.Utils.Constants;

namespace Automate.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> users;

        public UserServices(MongoDBServices mongoDBService)
        {
            users = mongoDBService.GetCollection<User>(DBConstants.USERS_COLLECTION_NAME);
        }

        public User? Authenticate(string username, string password)
        {
            User? user = users.Find(u => u.Username == username).FirstOrDefault();

            if (!VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public bool VerifyPassword(string password, string hashPassword) => BC.Verify(password, hashPassword);

        public string HashPassword(string password) => BC.HashPassword(password);

        public void RegisterUser(User user)
        {
            users.InsertOne(user);
        }
    }
}
