using Automate.Models;
using Automate.Utils.Constants;

namespace Automate.Tests.Models
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void SetUsername_ValueIsNull_ThrowArgumentNullException()
        {
            User user = new User();

            Action action = () => user.Username = null!;

            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void SetUsername_ValueIsEmptyString_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Username = "";

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void SetUsername_ValueIsEmptyStringWithWhiteSpace_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Username = "        ";

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void SetUsername_ValueIsValid_UsernameIsCorrectlySet()
        {
            const string username = "username";
            User user = new User();

            user.Username = username;

            Assert.AreEqual(username, user.Username);
        }

        [TestMethod]
        public void SetPassword_ValueIsNull_ThrowArgumentNullException()
        {
            User user = new User();

            Action action = () => user.Password = null!;

            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void SetPassword_ValueIsEmpty_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Password = "";

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void SetPassword_ValueIsEmptyWithWhiteSpace_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Password = "       ";

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void SetPassword_ValueIsValid_PasswordIsCorrectlySet()
        {
            const string password = "password";
            User user = new User();

            user.Password = password;

            Assert.AreEqual(password, user.Password);
        }

        [TestMethod]
        public void SetRole_ValueIsNull_ThrowArgumentNullException()
        {
            User user = new User();

            Action action = () => user.Role = null!;

            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void SetRole_ValueIsNotEmployeeOrAdmin_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Role = "Other";

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void SetRole_ValueIsEmployee_RoleIsCorrectlySet()
        {
            User user = new User();

            user.Role = RoleConstants.EMPLOYEE;

            Assert.AreEqual(RoleConstants.EMPLOYEE, user.Role);
        }

        [TestMethod]
        public void SetRole_ValueIsAdmin_RoleIsCorrectlySet()
        {
            User user = new User();

            user.Role = RoleConstants.ADMIN;

            Assert.AreEqual(RoleConstants.ADMIN, user.Role);
        }
    }
}
