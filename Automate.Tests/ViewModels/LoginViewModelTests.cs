using Automate.Abstract.Services;
using Automate.Models;
using Automate.ViewModels;
using Moq;
using System.ComponentModel;
using System.Security.RightsManagement;
using System.Windows;

namespace Automate.Tests.ViewModels
{
    [TestClass]
    public class LoginViewModelTests
    {
        private LoginViewModel loginViewModel;
        private Mock<Window> mockWindow;
        private Mock<IUserServices> mockUserService;
        private Mock<IMongoDBServices> mongoDbServicesMock;
        private Mock<PropertyChangedEventHandler> mockPropertyChanged;

        private readonly User? NULL_USER = null;
        public LoginViewModelTests() 
        {
            mockUserService = new Mock<IUserServices>();
            mongoDbServicesMock = new Mock<IMongoDBServices>();
            mockWindow = new Mock<Window>();
            mockPropertyChanged = new Mock<PropertyChangedEventHandler>();
            loginViewModel = new LoginViewModel(mockWindow.Object, mongoDbServicesMock.Object, mockUserService.Object);
        }

        [TestMethod]
        public void SetUsername_ValueIsValid_UsernameIsCorrectlySet()
        {
            const string username = "username";
            loginViewModel.Username = username;

            Assert.AreEqual(username, loginViewModel.Username);
        }

        [TestMethod]
        public void onPropertyChanged_change_propertyUsername()
        {
            loginViewModel.Username = "username";

            mockPropertyChanged.Verify(x => x.Invoke(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        }

        [TestMethod]
        public void SetPassword_ValueIsValid_UsernameIsCorrectlySet()
        {
            const string password = "password";
            loginViewModel.Password = password;

            Assert.AreEqual(password, loginViewModel.Password);
        }

        [TestMethod]
        public void onPropertyChanged_change_propertyPassword()
        {
            loginViewModel.Password = "password";

            mockPropertyChanged.Verify(x => x.Invoke(It.IsAny<object>(), It.IsAny<PropertyChangedEventArgs>()), Times.Once());
        }

        [TestMethod]
        public void Authenticate_UserExists_AuthenticateUser()
        {
            User validUser = new User { Username = "validUser" };
            const string username = "username";
            const string password = "password";
            loginViewModel.Username = username;
            loginViewModel.Password = password;
            mockUserService.Setup(us => us.Authenticate(username, password)).Returns(validUser);

            loginViewModel.Authenticate();

            mockUserService.Verify(us => us.Authenticate(username, password), Times.Once);
        }

        [TestMethod]
        public void Authenticate_UserDoesNotExist_AddsErrorMessage()
        {
            const string username = "username";
            const string wrongPassword = "wrongPassword";

            loginViewModel.Username = username;
            loginViewModel.Password = wrongPassword;
            mockUserService.Setup(us => us.Authenticate(username, wrongPassword)).Returns(NULL_USER);

            loginViewModel.Authenticate();

            mockUserService.Verify(us => us.Authenticate(username, wrongPassword), Times.Once);
        }
    }
}