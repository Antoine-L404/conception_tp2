using Automate.Models;
using Automate.Utils.Constants;

namespace Automate.TestsNUnit.Models
{
    public class UserTests
    {
        [Test]
        public void SetUsername_ValueIsNull_ThrowArgumentNullException()
        {
            User user = new User();

            Action action = () => user.Username = null!;

            Assert.Throws<ArgumentNullException>(() => action());
        }

        [Test]
        public void SetUsername_ValueIsEmptyString_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Username = "";

            Assert.Throws<ArgumentException>(() => action());
        }

        [Test]
        public void SetUsername_ValueIsEmptyStringWithWhiteSpace_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Username = "        ";

            Assert.Throws<ArgumentException>(() => action());
        }

        [Test]
        public void SetUsername_ValueIsValid_UsernameIsCorrectlySet()
        {
            const string username = "username";
            User user = new User();

            user.Username = username;

            Assert.That(user.Username, Is.EqualTo(username));
        }

        [Test]
        public void SetPassword_ValueIsNull_ThrowArgumentNullException()
        {
            User user = new User();

            Action action = () => user.Password = null!;

            Assert.Throws<ArgumentNullException>(() => action());
        }

        [Test]
        public void SetPassword_ValueIsEmpty_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Password = "";

            Assert.Throws<ArgumentException>(() => action());
        }

        [Test]
        public void SetPassword_ValueIsEmptyWithWhiteSpace_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Password = "       ";

            Assert.Throws<ArgumentException>(() => action());
        }

        [Test]
        public void SetPassword_ValueIsValid_PasswordIsCorrectlySet()
        {
            const string password = "password";
            User user = new User();

            user.Password = password;

            Assert.That(user.Password, Is.EqualTo(password));
        }

        [Test]
        public void SetRole_ValueIsNull_ThrowArgumentNullException()
        {
            User user = new User();

            Action action = () => user.Role = null!;

            Assert.Throws<ArgumentNullException>(() => action());
        }

        [Test]
        public void SetRole_ValueIsNotEmployeeOrAdmin_ThrowArgumentException()
        {
            User user = new User();

            Action action = () => user.Role = "Other";

            Assert.Throws<ArgumentException>(() => action());
        }

        [Test]
        public void SetRole_ValueIsEmployee_RoleIsCorrectlySet()
        {
            User user = new User();

            user.Role = RoleConstants.EMPLOYEE;

            Assert.That(user.Role, Is.EqualTo(RoleConstants.EMPLOYEE));
        }

        [Test]
        public void SetRole_ValueIsAdmin_RoleIsCorrectlySet()
        {
            User user = new User();

            user.Role = RoleConstants.ADMIN;

            Assert.That(user.Role, Is.EqualTo(RoleConstants.ADMIN));
        }
    }
}
