using Automate.Services;
using Automate.Utils.Constants;

namespace Automate.Utils
{
    public static class Environment
    {
        public static MongoDBServices mongoService = new MongoDBServices(DBConstants.DB_NAME);
        public static UserServices userServices = new UserServices(mongoService);
    }
}
