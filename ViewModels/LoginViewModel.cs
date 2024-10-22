using Automate.Services;
using Automate.Services.Commands;
using Automate.Utils;
using Automate.Utils.Constants;
using Automate.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly MongoDBServices mongoService;
        private readonly UserServices userServices;
        private readonly NavigationUtils navigationUtils;

        private string? username;
        private string? password;
        
        private Window window;

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public ICommand AuthenticateCommand { get; }
        public bool HasErrors => errors.Count > 0;
        public bool HasPasswordErrors => errors.ContainsKey(nameof(Password)) && errors[nameof(Password)].Any();

        public LoginViewModel(Window openedWindow)
        {
            mongoService = new MongoDBServices(DBConstants.DB_NAME);
            userServices = new UserServices(mongoService);
            AuthenticateCommand = new RelayCommand(Authenticate);

            navigationUtils = new NavigationUtils();

            window = openedWindow;
        }

        public string? Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
                ValidateProperty(nameof(Username));
            }
        }

        public string? Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                ValidateProperty(nameof(Password));
            }
        }

        public string ErrorMessages
        {
            get
            {
                var allErrors = new List<string>();
                foreach (var errorList in errors.Values)
                {
                    allErrors.AddRange(errorList);
                }
                // Retirer les chaînes vides et nulles
                allErrors.RemoveAll(error => string.IsNullOrWhiteSpace(error));

                return string.Join("\n", allErrors); // Joint les erreurs par une nouvelle ligne
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Authenticate()
        {
            ValidateProperty(nameof(Username));
            ValidateProperty(nameof(Password));

            if (!HasErrors)
            {
                var user = userServices.Authenticate(Username, Password);
                if (user == null)
                {
                    AddError("Username", "Nom d'utilisateur ou mot de passe invalide");
                    AddError("Password", "");
                    Trace.WriteLine("invalid");
                }
                else
                {
                    navigationUtils.NavigateTo<AccueilWindow>();
                    navigationUtils.Close(window);
                    Trace.WriteLine("logged in");
                }

            }
        }

        private void ValidateProperty(string? propertyName)
        {
            switch (propertyName)
            {
                case nameof(Username):
                    if (string.IsNullOrEmpty(Username))
                    {
                        AddError(nameof(Username), "Le nom d'utilisateur ne peut pas être vide.");
                    }
                    else
                    {
                        RemoveError(nameof(Username));
                    }
                    break;

                case nameof(Password):
                    if (string.IsNullOrEmpty(Password))
                    {
                        AddError(nameof(Password), "Le mot de passe ne peut pas être vide.");
                    }
                    else
                    {
                        RemoveError(nameof(Password));
                    }
                    break;
            }
        }

        private void AddError(string propertyName, string errorMessage)
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
            // Notifier les changements des propriétés
            OnPropertyChanged(nameof(ErrorMessages));
            OnPropertyChanged(nameof(HasPasswordErrors));
        }

        private void RemoveError(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                errors.Remove(propertyName);
               ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName)); 
            }
            // Notifier les changements des propriétés
            OnPropertyChanged(nameof(ErrorMessages));
            OnPropertyChanged(nameof(HasPasswordErrors));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<string>();
            }

            return errors[propertyName];
        }

    }
}
