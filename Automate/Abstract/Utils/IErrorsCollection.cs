using System.Collections;

namespace Automate.Abstract.Utils
{
    public interface IErrorsCollection
    {
        void AddError(string propertyName, string errorMessage);

        void RemoveError(string propertyName);

        IEnumerable GetErrors(string? propertyName);

        string GetAllErrorMessages();

        bool ContainsAnyError();

        bool ContainsError(string key);
    }
}
