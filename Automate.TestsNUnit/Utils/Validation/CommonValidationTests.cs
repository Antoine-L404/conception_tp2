using Automate.Abstract.Utils;
using Automate.Utils.Validation;
using Moq;

namespace Automate.TestsNUnit.Utils.Validation
{
    public class CommonValidationTests
    {
        private Mock<IErrorsCollection> errorCollectionMock;
        private IErrorsCollection errorCollection;
        private Action emptyAction;
        private string actionResult = "";
        private readonly string ACTION_VALUE = "invoked";

        private readonly string PROPERTY_NAME = "propertyName";
        private readonly string PROPERTY = "property";
        private readonly string ERROR_MESSAGE = "errorMessage";

        [SetUp]
        public void SetUp()
        {
            emptyAction = () => { actionResult = ACTION_VALUE; };
            errorCollectionMock = new Mock<IErrorsCollection>();
            errorCollection = errorCollectionMock.Object;
        }

        [Test]
        public void ValidateNullOrEmpty_PropertyIsNull_ErrorIsAdded()
        {
            CommonValidation.ValidateStringNullOrEmpty(PROPERTY_NAME, null, ERROR_MESSAGE, errorCollection, emptyAction);

            errorCollectionMock.Verify(x => x.AddError(PROPERTY_NAME, ERROR_MESSAGE), Times.Once());
        }

        [Test]
        public void ValidateNullOrEmpty_PropertyIsNull_ActionIsInvoked()
        {
            actionResult = "";

            CommonValidation.ValidateStringNullOrEmpty(PROPERTY_NAME, null, ERROR_MESSAGE, errorCollection, emptyAction);

            Assert.That(actionResult, Is.EqualTo(ACTION_VALUE));
        }

        [Test]
        public void ValidateNullOrEmpty_PropertyIsEmpty_ErrorIsAdded()
        {
            CommonValidation.ValidateStringNullOrEmpty(PROPERTY_NAME, string.Empty, ERROR_MESSAGE, errorCollection, emptyAction);

            errorCollectionMock.Verify(x => x.AddError(PROPERTY_NAME, ERROR_MESSAGE), Times.Once());
        }

        [Test]
        public void ValidateNullOrEmpty_PropertyIsEmpty_ActionIsInvoked()
        {
            actionResult = "";

            CommonValidation.ValidateStringNullOrEmpty(PROPERTY_NAME, string.Empty, ERROR_MESSAGE, errorCollection, emptyAction);

            Assert.That(actionResult, Is.EqualTo(ACTION_VALUE));
        }

        [Test]
        public void ValidateNullOrEmpty_ValidProperty_ErrorIsRemoved()
        {
            CommonValidation.ValidateStringNullOrEmpty(PROPERTY_NAME, PROPERTY, ERROR_MESSAGE, errorCollection, emptyAction);

            errorCollectionMock.Verify(x => x.RemoveError(PROPERTY_NAME), Times.Once());
        }

        [Test]
        public void ValidateNullOrEmpty_ValidProperty_ActionIsInvoked()
        {
            actionResult = "";

            CommonValidation.ValidateStringNullOrEmpty(PROPERTY_NAME, string.Empty, ERROR_MESSAGE, errorCollection, emptyAction);

            Assert.That(actionResult, Is.EqualTo(ACTION_VALUE));
        }
    }
}
