using System;
using System.Windows.Input;

namespace ImageTest
{
    public class Command : ICommand
    {
        private readonly Action _execute;

        public Command(Action onExecute)
        {
            _execute = onExecute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
