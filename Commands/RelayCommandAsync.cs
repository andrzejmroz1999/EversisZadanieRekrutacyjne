using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EversisZadanieRekrutacyjne.Commands
{
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommandAsync(Func<object, Task> execute)
            : this(execute, null)
        {
        }

        public RelayCommandAsync(Func<object, Task> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        private async Task ExecuteAsync(object parameter)
        {
            await _execute(parameter);
        }
    }
}