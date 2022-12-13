using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MyChat.Infrastructure
{
    internal class RelayCommand : ICommand
    {

        Predicate<object> _canExecuteMethod;
        Action<object> _executeMethod;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod = null)
        {
            _canExecuteMethod = canExecuteMethod;
            _executeMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }
    }
}
