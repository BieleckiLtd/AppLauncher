using AppLauncher.Enums;
using AppLauncher.Utils;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AppLauncher.ViewModels
{


    public class SplashScreenViewModel
    {
        public IStatusNotifier StatusNotifier { get; private set; } = new StatusNotifierViewModel();
        public StartupManagerExitCode StartupManagerExitCode { get; private set; }
        public IConfigurationProvider ConfigurationProvider { get; private set; }

        public SplashScreenViewModel()
        {
            ConfigurationProvider = Utils.ConfigurationProvider.Load();

            var sm = new StartupManager(ConfigurationProvider, StatusNotifier);
            StartAsync(sm);
        }


        /// <summary>
        /// Asynchronously initializes startup flow
        /// </summary>
        public async void StartAsync(StartupManager sm)
        {
            var startupResults = await sm.ManageStartupAsync();
            StartupManagerExitCode = startupResults;

            switch (startupResults)
            {
                case StartupManagerExitCode.Success:
                    StatusNotifier.Print("Starting...");
                    break;
                case StartupManagerExitCode.UpdateFailedButNotRequired:
                    StatusNotifier.Print("Update not currently possible, starting installed version...");
                    break;
                case StartupManagerExitCode.UpdateRequiredButFailed:
                    // Configuration requires to make sure app is always most recent
                    StatusNotifier.Print("Update not possible. Application cannot be started.");
                    StatusNotifier.IsTryAgainVisible = true;
                    MessageBox.Show(
                        messageBoxText: "Application could not be started. Check your network and try again.",
                        caption: "Application not started",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);
                    break;
                case StartupManagerExitCode.InstallationFailed:
                    StatusNotifier.Print("Installation failed. Application cannot be started.");
                    StatusNotifier.IsTryAgainVisible = true;
                    MessageBox.Show(
                        messageBoxText: "Application could not be installed. Check your network and try again.",
                        caption: "Application not started",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
            if (ConfigurationProvider.IsLoggerOn)
            {
                Logger.DumpToFile();
            }
            // close
            if (startupResults == StartupManagerExitCode.Success ||
                startupResults == StartupManagerExitCode.UpdateFailedButNotRequired)
            {
                StartupManager.StartProcess(ConfigurationProvider);
                CloseWithDelayAsync(2000);
            }
        }

        /// <summary>
        /// Closes this application after specified delay
        /// </summary>
        /// <param name="delayInMilliseconds">Delay time in milliseconds</param>
        private async void CloseWithDelayAsync(int delayInMilliseconds = 0)
        {
            await Task.Run(() => Thread.Sleep(delayInMilliseconds));
            Application.Current.MainWindow.Close();
        }
    }


}
