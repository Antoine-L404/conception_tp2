using Automate.Abstract.Services;
using Automate.Abstract.Utils;
using Automate.Models;
using Automate.Services.Commands;
using Automate.Utils.Validation;
using Automate.Views;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Automate.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly IUserServices userServices;
        private readonly INavigationUtils navigationUtils;

        private string? username;
        private string? password;


        private Window window;

        private ErrorsCollection errorsCollection;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public ICommand AuthenticateCommand { get; }
        public bool HasErrors => errorsCollection.ContainsAnyError();
        public bool HasPasswordErrors => errorsCollection.ContainsError(nameof(Password));

        public LoginViewModel(Window openedWindow, IUserServices userServices, INavigationUtils navigationUtils)
        {
            this.userServices = userServices;
            this.navigationUtils = navigationUtils;
            AuthenticateCommand = new RelayCommand(Authenticate);


            errorsCollection = new ErrorsCollection(ErrorsChanged);

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
            get => errorsCollection.GetAllErrorMessages();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName) => errorsCollection.GetErrors(propertyName);

        public void Authenticate()
        {
            ValidateUsername();
            ValidatePassword();

            if (HasErrors)
                return;

            User? user = userServices.Authenticate(Username!, Password!);
            if (user == null)
            {
                errorsCollection.AddError(nameof(Username), "Nom d'utilisateur ou mot de passe invalide");
                NotifyErrorChange();
                Trace.WriteLine("invalid");
            }
            else
            {
                navigationUtils.NavigateToAndCloseCurrentWindow<HomeWindow>(window);
                Trace.WriteLine("logged in");
            }
        }

        private void NotifyErrorChange()
        {
            OnPropertyChanged(nameof(ErrorMessages));
            OnPropertyChanged(nameof(HasPasswordErrors));
        }

        private void ValidateUsername()
        {
            CommonValidation.ValidateNullOrEmpty(
                nameof(Username), 
                Username, 
                "Le nom d'utilisateur ne peut pas être vide.", 
                errorsCollection, 
                NotifyErrorChange);
        }

        private void ValidatePassword()
        {
            CommonValidation.ValidateNullOrEmpty(
                nameof(Password), 
                Password,
                "Le mot de passe ne peut pas être vide.", 
                errorsCollection,
                NotifyErrorChange);
        }
    }
}
