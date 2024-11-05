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
    }
}
