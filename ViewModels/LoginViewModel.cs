using Automate.Services;
using Automate.Services.Commands;
using Automate.Utils;
using Automate.Utils.Constants;
using Automate.Views;
using System;
using System.Collections;
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

        private ErrorUtils errorUtils;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public ICommand AuthenticateCommand { get; }
        public bool HasErrors => errorUtils.errors.Count > 0;
        public bool HasPasswordErrors => 
            errorUtils.errors.ContainsKey(nameof(Password)) && errorUtils.errors[nameof(Password)].Any();

        public LoginViewModel(Window openedWindow)
        {
            mongoService = new MongoDBServices(DBConstants.DB_NAME);
            userServices = new UserServices(mongoService);
            AuthenticateCommand = new RelayCommand(Authenticate);

            navigationUtils = new NavigationUtils();

            errorUtils = new ErrorUtils();

            window = openedWindow;
        }

        public string? Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
                ValidateUsername();
            }
        }

        public string? Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                ValidatePassword();
            }
        }

        public string ErrorMessages
        {
            get => errorUtils.GetAllErrorMessages();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName) => errorUtils.GetErrors(propertyName);

        public void Authenticate()
        {
            ValidateUsername();
            ValidatePassword();

            if (!HasErrors)
            {
                var user = userServices.Authenticate(Username, Password);
                if (user == null)
                {
                    errorUtils.AddError(nameof(Username), "Nom d'utilisateur ou mot de passe invalide", ErrorsChanged);
                    NotifyErrorChange();
                    Trace.WriteLine("invalid");
                }
                else
                {
                    navigationUtils.NavigateToAndCloseCurrentWindow<AccueilWindow>(window);
                    Trace.WriteLine("logged in");
                }
            }
        }

        private void NotifyErrorChange()
        {
            OnPropertyChanged(nameof(ErrorMessages));
            OnPropertyChanged(nameof(HasPasswordErrors));
        }

        #region Validation
        private void ValidateUsername()
        {
            if (string.IsNullOrEmpty(Username))
            {
                errorUtils.AddError(nameof(Username), "Le nom d'utilisateur ne peut pas être vide.", ErrorsChanged);
                NotifyErrorChange();
            }
            else
            {
                errorUtils.RemoveError(nameof(Username), ErrorsChanged);
                NotifyErrorChange();
            }
        }

        private void ValidatePassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                errorUtils.AddError(nameof(Password), "Le mot de passe ne peut pas être vide.", ErrorsChanged);
                NotifyErrorChange();
            }
            else
            {
                errorUtils.RemoveError(nameof(Password), ErrorsChanged);
                NotifyErrorChange();
            }
        }
        #endregion
    }
}
