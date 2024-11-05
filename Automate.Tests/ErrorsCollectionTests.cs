using Automate.Utils.Validation;
using System.ComponentModel;

namespace Automate.Tests
{
    [TestClass]
    public class ErrorsCollectionTests
    {
        private ErrorsCollection errorsCollection;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        
        private readonly string ERROR_MESSAGE_1 = "unique error message1";
        private readonly string ERROR_MESSAGE_2 = "unique error message2";
        private readonly string EMPTY_ERROR_MESSAGE = string.Empty;

        public ErrorsCollectionTests()
        {
            errorsCollection = new ErrorsCollection(ErrorsChanged);
        }

        [TestMethod]
        public void AddError_AddNewKey_AddedPropertyIsNotNull()
        {
            const string PROPERTY_NAME = "Unique1";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddError_AddNewKey_AddedPropertyValueIsCorrect()
        {
            const string PROPERTY_NAME = "Unique2";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.AreEqual(ERROR_MESSAGE_1, result![0]);
        }

        [TestMethod]
        public void AddError_AddNewKey_AddedPropertyCountIsOne()
        {
            const string PROPERTY_NAME = "Unique2";
            const int EXPECTED_COUNT = 1;

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.AreEqual(EXPECTED_COUNT, result!.Count);
        }

        [TestMethod]
        public void AddError_AddExistantKey_AddedPropertyIsNotNull()
        {
            const string PROPERTY_NAME = "Unique3";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddError_AddExistantKey_AddedPropertyValueIsCorrect()
        {
            const string PROPERTY_NAME = "Unique4";

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.AreEqual(ERROR_MESSAGE_2, result![1]);
        }

        [TestMethod]
        public void AddError_AddExistantKey_AddedPropertyCountIsOneMore()
        {
            const string PROPERTY_NAME = "Unique5";
            const int EXPECTED_COUNT = 2;

            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;
            Assert.AreEqual(EXPECTED_COUNT, result!.Count);
        }

        [TestMethod]
        public void RemoveError_InexistantKey_DoesNothing()
        {
            const string PROPERTY_NAME = "Unique6";

            errorsCollection.RemoveError(PROPERTY_NAME);

            var result = errorsCollection.GetErrors(PROPERTY_NAME);
            Assert.AreEqual(Enumerable.Empty<string>(), result);
        }

        [TestMethod]
        public void RemoveError_ExistantKey_RemoveTheKey()
        {
            const string PROPERTY_NAME = "Unique7";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            errorsCollection.RemoveError(PROPERTY_NAME);

            var result = errorsCollection.GetErrors(PROPERTY_NAME);
            Assert.AreEqual(Enumerable.Empty<string>(), result);
        }

        [TestMethod]
        public void GetErrors_InexistantKey_ReturnEmptyEnumerable()
        {
            const string PROPERTY_NAME = "Unique8";

            var result = errorsCollection.GetErrors(PROPERTY_NAME);
            Assert.AreEqual(Enumerable.Empty<string>(), result);
        }

        [TestMethod]
        public void GetErrors_ExistantKey_ReturnValueIsNotEmpty()
        {
            const string PROPERTY_NAME = "Unique9";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            var result = errorsCollection.GetErrors(PROPERTY_NAME);

            Assert.AreNotEqual(Enumerable.Empty<string>(), result);
        }

        [TestMethod]
        public void GetErrors_ExistantKey_ReturnValueIsCorrect()
        {
            const string PROPERTY_NAME = "Unique9";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;

            Assert.AreEqual(ERROR_MESSAGE_1, result![0]);
        }

        [TestMethod]
        public void GetErrors_ExistantKey_ReturnValueCountIsCorrect()
        {
            const string PROPERTY_NAME = "Unique9";
            const int EXPECTED_COUNT = 1;
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            List<string>? result = errorsCollection.GetErrors(PROPERTY_NAME) as List<string>;

            Assert.AreEqual(EXPECTED_COUNT, result!.Count);
        }

        [TestMethod]
        public void GetAllErrorMessages_NoErrorMessage_ReturnEmptyString()
        {
            errorsCollection = new ErrorsCollection(ErrorsChanged);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetAllErrorMessages_ContainsErrorMessages_ReturnNotEmptyString()
        {
            const string PROPERTY_NAME = "Unique10";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.AreNotEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetAllErrorMessages_ContainsOneErrorMessage_ReturnErrorMessage()
        {
            const string PROPERTY_NAME = "Unique11";
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.AreEqual(ERROR_MESSAGE_1, result);
        }

        [TestMethod]
        public void GetAllErrorMessages_ContainsManyErrorMessages_ReturnErrorMessagesWithCorrectFormat()
        {
            const string PROPERTY_NAME = "Unique11";
            string expectedResult = string.Join("\n", ERROR_MESSAGE_1, ERROR_MESSAGE_2);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_1);
            errorsCollection.AddError(PROPERTY_NAME, ERROR_MESSAGE_2);

            string result = errorsCollection.GetAllErrorMessages();

            Assert.AreEqual(expectedResult, result);
        }
    }
}
