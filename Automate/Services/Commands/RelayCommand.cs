using System;
using System.Windows.Input;

namespace Automate.Services.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object>? executeWithParam;
        private readonly Action? executeWithoutParam;
        private readonly Func<object, bool>? canExecuteWithParam;
        private readonly Func<bool>? canExecuteWithoutParam;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object> executeWithParam, Func<object, bool>? canExecuteWithParam = null)
            : this (executeWithParam, null, canExecuteWithParam, null)
        { }

        public RelayCommand(Action executeWithoutParam, Func<bool>? canExecuteWithoutParam = null)
            : this (null, executeWithoutParam, null, canExecuteWithoutParam)
        { }

        public RelayCommand(Action<object>? executeWithParam, Action? executeWithoutParam, 
            Func<object, bool>? canExecuteWithParam, Func<bool>? canExecuteWithoutParam)
        {
            this.executeWithParam = executeWithParam;
            this.executeWithoutParam = executeWithoutParam;
            this.canExecuteWithParam = canExecuteWithParam;
            this.canExecuteWithoutParam = canExecuteWithoutParam;
        }

        public bool CanExecute(object? parameter)
        {
            if (canExecuteWithParam != null && parameter != null)
            {
                return canExecuteWithParam(parameter);
            }

            return canExecuteWithoutParam == null || canExecuteWithoutParam();
        }

        public void Execute(object? parameter)
        {
            if (executeWithParam != null && parameter != null)
            {
                executeWithParam(parameter);
            }
            else
            {
                executeWithoutParam?.Invoke();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}