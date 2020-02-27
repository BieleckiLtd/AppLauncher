using System;
using System.Windows;
using System.Windows.Input;

namespace AppLauncher.Commands
{
    public class ApplicationMinimize : ICommand
    {
        public void Execute(object parameter)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public bool CanExecute(object parameter)
        {
            return Application.Current != null && Application.Current.MainWindow != null;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
