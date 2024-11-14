using Automate.Utils.Validation;
using Moq;
using System.ComponentModel;

namespace Automate.TestsNUnit.Utils.Validation
{
    public class ErrorsCollectionTests
    {
        private ErrorsCollection errorsCollection;
        Mock<EventHandler<DataErrorsChangedEventArgs>> errorsChangedMock;

        private readonly string ERROR_MESSAGE_1 = "unique error message1";
        private readonly string ERROR_MESSAGE_2 = "unique error message2";

        [SetUp]
        public void SetUp()
        {
            errorsChangedMock = new Mock<EventHandler<DataErrorsChangedEventArgs>>();
            errorsCollection = new ErrorsCollection(errorsChangedMock.Object);
        }

        [Test]
        public void AddError_AddNewKey_AddedPropertyIsNotNull()
        {
            const string PROPERTY_NAME = "Unique1";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddError_AddNewKey_AddedPropertyValueIsCorrect()
        {
            const string PROPERTY_NAME = "Unique2";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.That(result![0], Is.EqualTo(ERROR_MESSAGE_1));
        }

        [Test]
        public void AddError_AddNewKey_AddedPropertyCountIsOne()
        {
            const string PROPERTY_NAME = "Unique2";
            const int EXPECTED_COUNT = 1;

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.That(result!.Count, Is.EqualTo(EXPECTED_COUNT));
        }

        [Test]
        public void AddError_AddExistantKey_AddedPropertyIsNotNull()
        {
            const string PROPERTY_NAME = "Unique3";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddError_AddExistantKey_AddedPropertyValueIsCorrect()
        {
            const string PROPERTY_NAME = "Unique4";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.That(result![1], Is.EqualTo(ERROR_MESSAGE_2));
        }

        [Test]
        public void AddError_AddExistantKey_AddedPropertyCountIsOneMore()
        {
            const string PROPERTY_NAME = "Unique5";
            const int EXPECTED_COUNT = 2;

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.That(result!.Count, Is.EqualTo(EXPECTED_COUNT));

        }

        [Test]
        public void AddError_AddExistantKey_ErrorChangedEventIsInvoked()
        {
            const string PROPERTY_NAME = "Unique6";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            errorsChangedMock.Verify(
                x => x.Invoke(It.IsAny<object>(), It.IsAny<DataErrorsChangedEventArgs>()), Times.AtLeastOnce());
        }

        [Test]
        public void RemoveError_InexistantKey_DoesNothing()
        {
            const string PROPERTY_NAME = "Unique7";

            errorsCollection.RemoveError(PROPERTY_NAME);

            var result = errorsCollection.GetErrors(PROPERTY_NAME);
            Assert.That(result, Is.EqualTo(Enumerable.Empty<string>()));
        }

        [Test]
        public void RemoveError_ExistantKey_RemoveTheKey()
        {
            const string PROPERTY_NAME = "Unique8";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            errorsCollection.RemoveError(PROPERTY_NAME);

            var result = errorsCollection.GetErrors(PROPERTY_NAME);
            Assert.That(result, Is.EqualTo(Enumerable.Empty<string>()));
        }

        [Test]
        public void RemoveError_ExistantKey_ErrorChangedEventIsInvoked()
        {
            const string PROPERTY_NAME = "Unique9";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            errorsCollection.RemoveError(PROPERTY_NAME);

            errorsChangedMock.Verify(
                x => x.Invoke(It.IsAny<object>(), It.IsAny<DataErrorsChangedEventArgs>()), Times.AtLeastOnce());
        }

        [Test]
        public void GetErrors_InexistantKey_ReturnEmptyEnumerable()
        {
            const string PROPERTY_NAME = "Unique10";

            var result = errorsCollection.GetErrors(PROPERTY_NAME);
            Assert.That(result, Is.EqualTo(Enumerable.Empty<string>()));
        }

        [Test]
        public void GetErrors_ExistantKey_ReturnValueIsNotEmpty()
        {
            const string PROPERTY_NAME = "Unique11";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            var result = errorsCollection.GetErrors(PROPERTY_NAME);

            Assert.That(result, Is.Not.EqualTo(Enumerable.Empty<string>()));
        }

        [Test]
        public void GetErrors_ExistantKey_ReturnValueIsCorrect()
        {
            const string PROPERTY_NAME = "Unique12";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;

            Assert.That(result![0], Is.EqualTo(ERROR_MESSAGE_1));
        }

        [Test]
        public void GetErrors_ExistantKey_ReturnValueCountIsCorrect()
        {
            const string PROPERTY_NAME = "Unique13";
            const int EXPECTED_COUNT = 1;
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;

            Assert.That(result!.Count, Is.EqualTo(EXPECTED_COUNT));
        }

        [Test]
        public void GetAllErrorMessages_ContainsNoErrorMessage_ReturnEmptyString()
        {
            errorsCollection = new ErrorsCollection(errorsChangedMock.Object);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetAllErrorMessages_ContainsErrorMessages_ReturnNotEmptyString()
        {
            const string PROPERTY_NAME = "Unique14";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.That(result, Is.Not.EqualTo(string.Empty));
        }

        [Test]
        public void GetAllErrorMessages_ContainsOneErrorMessage_ReturnErrorMessage()
        {
            const string PROPERTY_NAME = "Unique15";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.That(result, Is.EqualTo(ERROR_MESSAGE_1));
        }

        [Test]
        public void GetAllErrorMessages_ContainsManyErrorMessages_ReturnErrorMessagesWithCorrectFormat()
        {
            const string PROPERTY_NAME = "Unique16";
            string expectedResult = string.Join("\n", ERROR_MESSAGE_1, ERROR_MESSAGE_2);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ContainsAnyError_ContainsNoError_ReturnFalse()
        {
            errorsCollection = new ErrorsCollection(errorsChangedMock.Object);

            bool result = errorsCollection.ContainsAnyError();

            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsAnyError_ContainsOneError_ReturnTrue()
        {
            const string PROPERTY_NAME = "Unique17";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            bool result = errorsCollection.ContainsAnyError();

            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsAnyError_ContainsManyErrors_ReturnTrue()
        {
            const string PROPERTY_NAME = "Unique18";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            bool result = errorsCollection.ContainsAnyError();

            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsError_InexistantKey_ReturnFalse()
        {
            const string PROPERTY_NAME = "Unique19";

            bool result = errorsCollection.ContainsError(PROPERTY_NAME);

            Assert.IsFalse(result);
        }

        [Test]
        public void ContainsError_ExistantKey_ReturnTrue()
        {
            const string PROPERTY_NAME = "Unique20";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            bool result = errorsCollection.ContainsError(PROPERTY_NAME);

            Assert.IsTrue(result);
        }
    }
}
