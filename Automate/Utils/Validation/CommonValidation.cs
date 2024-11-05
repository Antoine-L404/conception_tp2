using System;

namespace Automate.Utils.Validation
{
    public static class CommonValidation
    {
        public static void ValidateNullOrEmpty(string propertyName, string? property, string errorMessage,
            ErrorsCollection errorsCollection, Action notifyErrorsAction)
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
