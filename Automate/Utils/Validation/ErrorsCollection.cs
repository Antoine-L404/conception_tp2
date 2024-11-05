using Automate.Abstract.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Automate.Utils.Validation
{
    public class ErrorsCollection : IErrorsCollection
    {
        private Dictionary<string, List<string>> errors;
        private EventHandler<DataErrorsChangedEventArgs>? errorsChangedEvent;

        public ErrorsCollection(EventHandler<DataErrorsChangedEventArgs>? errorsChangedEvent)
        {
            errors = new Dictionary<string, List<string>>();
            this.errorsChangedEvent = errorsChangedEvent;
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors[propertyName] = new List<string>();
            }
            if (!errors[propertyName].Contains(errorMessage))
            {
                errors[propertyName].Add(errorMessage);
                errorsChangedEvent?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public void RemoveError(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
                errorsChangedEvent?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return errors[propertyName];
        }

        public string GetAllErrorMessages()
        {
            List<string> allErrors = new List<string>();
            foreach (var errorList in errors.Values)
            {
                allErrors.AddRange(errorList);
            }

            allErrors.RemoveAll(error => string.IsNullOrWhiteSpace(error));

            return string.Join("\n", allErrors);
        }

        public bool ContainsAnyError() => errors.Count > 0;

        public bool ContainsError(string key) => errors.ContainsKey(key) && errors[key].Any();
    }
}
