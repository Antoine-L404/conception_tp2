using Automate.Abstract.Services;
using Automate.Models;
using Automate.Services;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using Moq;
using System;

namespace Automate.Tests.Services
{
    [TestClass]
    public class UserServicesTests
    {
        private UserServices userServices;
        private Mock<IMongoDBServices> mongoDbServicesMock;
        private Mock<IMongoCollection<User>> userCollectionMock;
        private Mock<IFindFluent<User, User>> findFluentMock;
        private Mock<IAsyncCursor<User>> cursorMock;

        private readonly string UNKNOWN_USERNAME = "unknown";
        private readonly User? NULL_USER = null;

        public UserServicesTests() 
        {
            findFluentMock = new Mock<IFindFluent<User, User>>();
            cursorMock = new Mock<IAsyncCursor<User>>();
            
            userCollectionMock = new Mock<IMongoCollection<User>>();

            mongoDbServicesMock = new Mock<IMongoDBServices>();
            mongoDbServicesMock.Setup(x => x.GetCollection<User>(It.IsAny<string>())).Returns(userCollectionMock.Object);

            userServices = new UserServices(mongoDbServicesMock.Object);
        }

        [TestMethod]
        public void Authenticate()
        {
            //findFluentMock.Setup(x => x.FirstOrDefault(It.IsAny<CancellationToken>())).Returns(NULL_USER);
            //cursorMock.SetupSequence(_async => _async.MoveNext(default)).Returns(true).Returns(false);
            //cursorMock.SetupGet(_async => _async.Current).Returns(new List<User>() { NULL_USER});

            var username = "";
            var user = new User { /* initialize properties */ };
            var filter = Builders<User>.Filter.Eq(u => u.Username, username);

            //userCollectionMock.Setup(x => x.Find(u => u.Username == UNKNOWN_USERNAME, null)).Returns(findFluentMock.Object);
            //userCollectionMock.Setup(m => m.Find(filter, null))
            //    .Returns(new List<User> { user }.AsQueryable());

            userCollectionMock.Setup(m => m.Find(filter, null).FirstOrDefault(default))
                .Returns(user);


            userServices.Authenticate("", "");
        }
    }
}
