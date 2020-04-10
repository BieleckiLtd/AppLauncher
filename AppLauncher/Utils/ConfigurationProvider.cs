using AppLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace AppLauncher.Utils
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        #region Properties
        /// <summary>
        /// Name of the application that is being launched
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Version of the application that is being launched. This is visible only on the taskbar.
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Line of text that is being displayed under the application name.
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Splash screen background colour.
        /// Alpha channel is supported but not advised due to the aesthetic and performance concerns.
        /// </summary>
        public string Background { get; set; } = "#000000";

        /// <summary>
        /// Text and logo colour. Use colour contrasting with the background colour.
        /// Some foreground elements opacity is less then 1.0 so alpha channel in foreground should't be used.
        /// </summary>
        public string Foreground { get; set; } = "#FFFFFF";

        /// <summary>
        /// Splash screen width.
        /// </summary>
        public double WindowWidth { get; set; }

        /// <summary>
        /// Splash screen height.
        /// </summary>
        public double WindowHeight { get; set; }

        /// <summary>
        /// Location, where the zip file with the actual application is being stored.
        /// Users should have read access to that location.
        /// </summary>
        public string RemotePackage { get; set; }

        /// <summary>
        /// Folder in C:\Users\<user>\ to which actual application will be extracted.
        /// </summary>
        public string LocalFolder { get; set; }

        /// <summary>
        /// Flag that determines whether launcher should allow to start currently installed version
        /// of the application even if it has not been possible to verify whether installed version
        /// is the latest one.
        /// </summary>
        public bool RequireLatestVersion { get; set; }

        /// <summary>
        /// File name inside zip's root that will be executed after extraction
        /// </summary>
        public string ExecutableFile { get; set; }

        /// <summary>
        /// Allows to enable logging.
        /// </summary>
        public bool IsLoggerOn { get; set; }

        /// <summary>
        /// Logo paths, max width and max height.
        /// </summary>
        public ConfigurationProviderLogo Logo { get; set; }

        /// <summary>
        /// Gets user's profile directory.
        /// </summary>
        public string InstallationDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), LocalFolder);
        
        /// <summary>
        /// Gets combined Application name and version 
        /// </summary>
        public string ApplicationNameAndVersion => $"{ApplicationName} {ApplicationVersion}";

        /// <summary>
        /// Converts background colour string literal to System.Windows.Media.Color
        /// </summary>
        public Color BackgroundColour => (Color)ColorConverter.ConvertFromString(Background);

        /// <summary>
        /// Converts foreground colour string literal to System.Windows.Media.Color
        /// </summary>
        public Color ForegroundColour => (Color)ColorConverter.ConvertFromString(Foreground);

        /// <summary>
        /// LogoFillColour color is ForegroundColour with little transparency (A = 238)
        /// </summary>
        public Color LogoFillColour
        {
            get {
                var foreground10AlphaColour = ForegroundColour;
                foreground10AlphaColour.A = 238;
                return foreground10AlphaColour;
            }
        }

        /// <summary>
        /// ForegroundMainColour color is ForegroundColour with some transparency (A = 204)
        /// </summary>
        public Color ForegroundMainColour
        {
            get {
                var foreground10AlphaColour = ForegroundColour;
                foreground10AlphaColour.A = 204;
                return foreground10AlphaColour;
            }
        }

        /// <summary>
        /// ForegroundSecondaryColour color is ForegroundColour with some more transparency (A = 119)
        /// </summary>
        public Color ForegroundSecondaryColour
        {
            get {
                var foreground10AlphaColour = ForegroundColour;
                foreground10AlphaColour.A = 119;
                return foreground10AlphaColour;
            }
        }

        /// <summary>
        /// ForegroundTertiaryColour color is ForegroundColour with a lot transparency (A = 68)
        /// </summary>
        public Color ForegroundTertiaryColour
        {
            get {
                var foreground10AlphaColour = ForegroundColour;
                foreground10AlphaColour.A = 68;
                return foreground10AlphaColour;
            }
        }

        /// <summary>
        /// ForegroundWashedOutColour is ForegroundColour with almost full transparency (A = 16)
        /// </summary>
        public Color ForegroundWashedOutColour
        {
            get {
                var foreground10AlphaColour = ForegroundColour;
                foreground10AlphaColour.A = 16;
                return foreground10AlphaColour;
            }
        }

        #endregion Properties

        /// <summary>
        /// Saves to an xml file. 
        /// <param name="fileName">Non-default file name</param>
        /// </summary>
        public void Save(string fileName = "config.xml")
        {
            string fileLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(fileLocation, fileName);

            using var writer = new StreamWriter(filePath);
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(writer, this);
            writer.Flush();
        }

        /// <summary>
        /// Static method to load an xml file as new ConfigurationProvider
        /// </summary>
        /// <param name="fileName">Non-default file name</param>
        /// <returns>The ConfigurationProvider object created from the xml file</returns>
        public static IConfigurationProvider Load(string fileName = "config.xml")
        {
            string fileLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(fileLocation, fileName);

            using var stream = File.OpenRead(filePath);
            var serializer = new XmlSerializer(typeof(ConfigurationProvider));
            return serializer.Deserialize(stream) as ConfigurationProvider;
        }

    }
}
