using Automate.Abstract.Services;
using Automate.Models;
using Automate.Services;
using MongoDB.Driver;
using Moq;

namespace Automate.Tests.Services
{
    [TestClass]
    public class UserServicesTests
    {
        private UserServices userServices;
        private Mock<IMongoDBServices> mongoDbServicesMock;
        private Mock<IMongoCollection<User>> userCollectionMock;

        private readonly string UNKNOWN_USERNAME = "unknown";
        private readonly User? NULL_USER = null;

        public UserServicesTests() 
        {
            userCollectionMock = new Mock<IMongoCollection<User>>();

            mongoDbServicesMock = new Mock<IMongoDBServices>();
            mongoDbServicesMock.Setup(x => x.GetCollection<User>(It.IsAny<string>())).Returns(userCollectionMock.Object);

            userServices = new UserServices(mongoDbServicesMock.Object);
        }

        [TestMethod]
        public void Authenticate()
        {
            mongoDbServicesMock.Setup(x => x.GetOne(It.IsAny<IMongoCollection<User>>(), u => u.Username == UNKNOWN_USERNAME)).Returns(NULL_USER);

            userServices.Authenticate("", "");
        }
    }
}
