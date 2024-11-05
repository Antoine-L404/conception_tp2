using Automate.Abstract.Utils;
using System;

namespace Automate.Utils.Validation
{
    public static class CommonValidation
    {
        public static void ValidateNullOrEmpty(string propertyName, string? property, string errorMessage,
            IErrorsCollection errorsCollection, Action notifyErrorsAction)
        {
            if (string.IsNullOrEmpty(property))
            {
                errorsCollection.AddError(propertyName, errorMessage);
                notifyErrorsAction();
            }
            else
            {
                errorsCollection.RemoveError(propertyName);
                notifyErrorsAction();
            }
        }
    }
}
