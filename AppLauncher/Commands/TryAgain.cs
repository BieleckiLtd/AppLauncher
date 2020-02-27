using AppLauncher.Utils;
using AppLauncher.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace AppLauncher.Commands
{
    public class TryAgain : ICommand
    {
        public void Execute(object parameter)
        {
            if (parameter is SplashScreenViewModel ss)
            {
                ss.StatusNotifier.IsTryAgainVisible = false;
                var sm = new StartupManager(ss.ConfigurationProvider, ss.StatusNotifier);
                ss.StartAsync(sm);
            }

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
