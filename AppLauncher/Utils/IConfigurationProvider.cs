using AppLauncher.Models;
using System.Windows.Media;

namespace AppLauncher.Utils
{
    public interface IConfigurationProvider
    {
        string ApplicationName { get; set; }
        string ApplicationNameAndVersion { get; }
        string ApplicationVersion { get; set; }
        string Background { get; set; }
        Color BackgroundColour { get; }
        string ExecutableFile { get; set; }
        string Foreground { get; set; }
        Color ForegroundColour { get; }
        Color ForegroundMainColour { get; }
        Color ForegroundSecondaryColour { get; }
        Color ForegroundTertiaryColour { get; }
        Color ForegroundWashedOutColour { get; }
        string InstallationDirectory { get; }
        bool IsLoggerOn { get; set; }
        string LocalFolder { get; set; }
        ConfigurationProviderLogo Logo { get; set; }
        Color LogoFillColour { get; }
        string RemotePackage { get; set; }
        bool RequireLatestVersion { get; set; }
        string Subtitle { get; set; }
        double WindowHeight { get; set; }
        double WindowWidth { get; set; }

        void Save(string fileName);
    }
}