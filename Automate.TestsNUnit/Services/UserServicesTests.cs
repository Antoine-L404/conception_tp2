using Automate.Abstract.Services;
using Automate.Models;
using Automate.Services;
using MongoDB.Driver;
using Moq;
using System.Linq.Expressions;

namespace Automate.TestsNUnit.Services
{
    public class UserServicesTests
    {
        private UserServices userServices;
        private Mock<IMongoDBServices> mongoDbServicesMock;
        private Mock<IMongoCollection<User>> userCollectionMock;

        private readonly string UNKNOWN_USERNAME = "unknownUsername";
        private readonly string CORRECT_USERNAME = "laurentMetNous100STP";
        private readonly string WRONG_PASSWORD = "wrongPassword";
        private readonly string CORRECT_PASSWORD = "Qwerty123!";
        private readonly string HASHED_PASSWORD = "$2a$11$LoPIt6X.yxk0MLM8zvdpje1rpfbBs/6tYGtf5t4.EJUUYbMR3lx5K";

        private User? NULL_USER = null;
        private User USER;

        [SetUp]
        public void SetUp()
        {
            USER = new User() { Username = CORRECT_USERNAME, Password = HASHED_PASSWORD };

            userCollectionMock = new Mock<IMongoCollection<User>>();
            mongoDbServicesMock = new Mock<IMongoDBServices>();
            mongoDbServicesMock.Setup(x => x.GetCollection<User>(It.IsAny<string>())).Returns(userCollectionMock.Object);

            userServices = new UserServices(mongoDbServicesMock.Object);
        }

        [Test]
        public void Authenticate_UnknwonUsername_ReturnNull()
        {
            mongoDbServicesMock.Setup(
                x => x.GetOne(It.IsAny<IMongoCollection<User>>(), u => u.Username == UNKNOWN_USERNAME)).Returns(NULL_USER!);

            User? result = userServices.Authenticate(UNKNOWN_USERNAME, "");

            Assert.IsNull(result);
        }

        [Test]
        public void Authenticate_WrongPassword_ReturnNull()
        {
            mongoDbServicesMock.Setup(
                x => x.GetOne(It.IsAny<IMongoCollection<User>>(), It.IsAny<Expression<Func<User, bool>>>())).Returns(USER);

            User? result = userServices.Authenticate(CORRECT_USERNAME, WRONG_PASSWORD);

            Assert.IsNull(result);
        }

        [Test]
        public void Authenticate_CorrectPassword_ReturnUser()
        {
            mongoDbServicesMock.Setup(
                x => x.GetOne(It.IsAny<IMongoCollection<User>>(), It.IsAny<Expression<Func<User, bool>>>())).Returns(USER);

            User? result = userServices.Authenticate(CORRECT_USERNAME, CORRECT_PASSWORD);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Authenticate_CorrectPassword_ReturnUserWithCorrectInformations()
        {
            mongoDbServicesMock.Setup(
                x => x.GetOne(It.IsAny<IMongoCollection<User>>(), It.IsAny<Expression<Func<User, bool>>>())).Returns(USER);

            User? result = userServices.Authenticate(CORRECT_USERNAME, CORRECT_PASSWORD);

            Assert.That(result!.Username, Is.EqualTo(CORRECT_USERNAME));
            Assert.That(result!.Password, Is.EqualTo(HASHED_PASSWORD));
        }
    }
}
