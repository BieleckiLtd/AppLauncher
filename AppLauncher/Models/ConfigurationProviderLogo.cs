using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AppLauncher.Models
{
    public partial class ConfigurationProviderLogo
    {
        [XmlArrayItem("path", typeof(ConfigurationProviderLogoPathsDataPath))]
        public List<ConfigurationProviderLogoPathsDataPath> PathsData { get; set; }

        [XmlAttribute()]
        public double MaxWidth { get; set; }
        [XmlAttribute()]
        public double MaxHeight { get; set; }
    }

    public partial class ConfigurationProviderLogoPathsDataPath
    {
        [XmlAttribute("d")]
        public string PathData { get; set; }
    }


}
