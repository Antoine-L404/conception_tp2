using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Automate.Utils
{
    public class ErrorUtils
    {
        public Dictionary<string, List<string>> errors;

        public ErrorUtils() 
        {
            errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string propertyName, string errorMessage, EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors[propertyName] = new List<string>();
            }
            if (!errors[propertyName].Contains(errorMessage))
            {
                errors[propertyName].Add(errorMessage);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public void RemoveError(string propertyName, EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
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
    }
}
