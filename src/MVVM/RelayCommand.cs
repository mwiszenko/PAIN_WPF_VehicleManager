using System;
using System.Windows.Input;

namespace Vehicles.MVVM
{
    public class RelayCommand<T> : ICommand
    {
        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;
        public event EventHandler CanExecuteChanged;
        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(((T)parameter)) ?? true;
        }
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
