using Automate.Models;

namespace Automate.Abstract.Services
{
    public interface IUserServices
    {
        User? Authenticate(string username, string password);
        bool VerifyPassword(string password, string hashPassword);
    }
}