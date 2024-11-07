using Automate.Models;
using BC = BCrypt.Net.BCrypt;
using MongoDB.Driver;
using Automate.Utils.Constants;
using Automate.Abstract.Services;

namespace Automate.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> users;
        private readonly IMongoDBServices mongoDBService;

        public UserServices(IMongoDBServices mongoDBService)
        {
            this.mongoDBService = mongoDBService;
            users = mongoDBService.GetCollection<User>(DBConstants.USERS_COLLECTION_NAME);
        }

        public User? Authenticate(string username, string password)
        {
            User? user = mongoDBService.GetOne(users, u => u.Username == username);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.Password))
                return null;

            return user;
        }

        public bool VerifyPassword(string password, string hashPassword) => BC.Verify(password, hashPassword);
    }
}
