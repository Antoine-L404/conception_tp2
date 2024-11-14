using Automate.Abstract.Utils;
using System;

namespace Automate.Utils.Validation
{
    public static class CommonValidation
    {
        public static void ValidateStringNullOrEmpty(string propertyName, string? property, string errorMessage,
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

        public static bool ValidateNull<T>(string propertyName, T? property, string errorMessage,
             IErrorsCollection errorsCollection, Action notifyErrorsAction)
        {
            if (property == null)
            {
                errorsCollection.AddError(propertyName, errorMessage);
                notifyErrorsAction();
                return false;
            }
            else
            {
                errorsCollection.RemoveError(propertyName);
                notifyErrorsAction();
                return true;
            }
        }
    }
}
