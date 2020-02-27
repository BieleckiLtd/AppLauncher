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
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string Subtitle { get; set; }
        public string Background { get; set; } = "#000000";
        public string Foreground { get; set; } = "#FFFFFF";
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public string RemotePackage { get; set; }
        public string LocalFolder { get; set; }
        public bool RequireLatestVersion { get; set; }
        public string ExecutableFile { get; set; }
        public bool IsLoggerOn { get; set; }
        public ConfigurationProviderLogo Logo { get; set; }

        public string InstallationDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), LocalFolder);
        public string ApplicationNameAndVersion => $"{ApplicationName} {ApplicationVersion}";
        public Color BackgroundColour => (Color)ColorConverter.ConvertFromString(Background);
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

        /// <summary>
        /// Saves to an xml file
        /// </summary>
        public void Save()
        {
            string fileLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fileName = @"config.xml";
            string filePath = Path.Combine(fileLocation, fileName);

            using var writer = new StreamWriter(filePath);
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(writer, this);
            writer.Flush();
        }

        /// <summary>
        /// Loads an object from an xml file
        /// </summary>
        /// <returns>The object created from the xml file</returns>
        public static IConfigurationProvider Load()
        {
            string fileLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fileName = @"config.xml";
            string filePath = Path.Combine(fileLocation, fileName);

            using var stream = File.OpenRead(filePath);
            var serializer = new XmlSerializer(typeof(ConfigurationProvider));
            return serializer.Deserialize(stream) as ConfigurationProvider;
        }
    }
}
