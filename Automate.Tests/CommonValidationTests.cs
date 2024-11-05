using Automate.Abstract.Utils;
using Automate.Utils.Validation;
using Moq;

namespace Automate.Tests
{
    [TestClass]
    public class CommonValidationTests
    {
        private readonly string EMPTY_PROPERTY = string.Empty;
        private Mock<IErrorsCollection> errorCollectionMock;
        private IErrorsCollection errorCollection;
        private Action osef = () => { };

        public CommonValidationTests() 
        {
            errorCollectionMock = new Mock<IErrorsCollection>();
            errorCollection = errorCollectionMock.Object;
        }

        [TestMethod]
        public void ValidateNullOrEmpty_()
        {
            CommonValidation.ValidateNullOrEmpty("", null, "", errorCollection, osef);
        }
    }
}
