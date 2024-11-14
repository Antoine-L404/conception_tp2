using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Models;
using Automate.ViewModels;
using Automate.Views;
using Moq;
using System.ComponentModel;
using System.Windows;

namespace Automate.TestsNUnit.ViewModels
{
    [Apartment(ApartmentState.STA)]
    public class LoginViewModelTests
    {
        private LoginViewModel loginViewModel;
        private Mock<Window> mockWindow;
        private Mock<IUserServices> mockUserService;
        private Mock<PropertyChangedEventHandler> mockPropertyChanged;
        private Mock<INavigationUtils> mockNavigationUtils;

        private readonly User? NULL_USER = null;

        [SetUp]
        public void Setup()
        {
            mockUserService = new Mock<IUserServices>();
            mockWindow = new Mock<Window>();
            mockPropertyChanged = new Mock<PropertyChangedEventHandler>();
            mockNavigationUtils = new Mock<INavigationUtils>();

            loginViewModel = new LoginViewModel(mockWindow.Object, mockUserService.Object, mockNavigationUtils.Object);

            loginViewModel.PropertyChanged += mockPropertyChanged.Object;
        }

        [Test]
        public void SetUsername_ValueIsValid_UsernameIsCorrectlySet()
        {
            const string username = "username";

            loginViewModel.Username = username;

            Assert.That(loginViewModel.Username, Is.EqualTo(username));
        }

        [Test]
        public void SetUsername_ValueIsValid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "Username";

            loginViewModel.Username = "username";

            mockPropertyChanged.Verify(x => 
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)), 
                Times.Once()
            );
        }

        [Test]
        public void SetPassword_ValueIsValid_UsernameIsCorrectlySet()
        {
            const string password = "password";

            loginViewModel.Password = password;

            Assert.That(loginViewModel.Password, Is.EqualTo(password));
        }

        [Test]
        public void SetPassword_ValueIsValid_OnPropertyChangedIsInvoked()
        {
            const string argPropertyName = "Password";

            loginViewModel.Password = "password";

            mockPropertyChanged.Verify(x => 
                x.Invoke(It.IsAny<object>(), It.Is<PropertyChangedEventArgs>(args => args.PropertyName == argPropertyName)), 
                Times.Once()
            );
        }

        [Test]
        public void Authenticate_UserExists_CallAuthenticateUserService()
        {
            User validUser = new User { Username = "username", Password = "password" };
            const string username = "username";
            const string password = "password";

            loginViewModel.Username = username;
            loginViewModel.Password = password;
            mockUserService.Setup(us => us.Authenticate(username, password)).Returns(validUser);
            loginViewModel.Authenticate();

            mockUserService.Verify(us => us.Authenticate(username, password), Times.Once);
        }

        [Test]
        public void Authenticate_UserExists_NavigateToHomeWindow()
        {
            User validUser = new User { Username = "username", Password = "password" };
            const string username = "username";
            const string password = "password";

            loginViewModel.Username = username;
            loginViewModel.Password = password;
            mockUserService.Setup(us => us.Authenticate(username, password)).Returns(validUser);
            loginViewModel.Authenticate();

            mockNavigationUtils.Verify(
                x => x.NavigateToAndCloseCurrentWindow<HomeWindow>(It.IsAny<Window>()), Times.Once());
        }

        [Test]
        public void Authenticate_UserDoesNotExist_AddsErrorMessage()
        {
            const string username = "username";
            const string wrongPassword = "wrongPassword";

            loginViewModel.Username = username;
            loginViewModel.Password = wrongPassword;
            mockUserService.Setup(us => us.Authenticate(username, wrongPassword)).Returns(NULL_USER);
            loginViewModel.Authenticate();

            Assert.That(loginViewModel.HasErrors, Is.EqualTo(true));
        }
    }
}