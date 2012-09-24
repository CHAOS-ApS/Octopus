using System;
using Geckon.Octopus.Plugin.Core;
using Geckon.Serialization.Xml;
using Zen = Zencoder;

namespace CHAOS.Octopus.Plugins.Transcoding.Zencoder
{
    public abstract class ZencoderBasePlugin : APlugin
    {
        #region Properties

        [Element("AccessKey")]
        public string AccessKey { get; set; }

        [Element("BaseURL")]
        public string BaseUrl { get; set; }

        [Element("Public")]
        public bool? Public { get; set; }

        [Element("SourceFilePath")]
        public string SourceFilePath { get; set; }

        [Element("DestinationFilePath")]
        public string DestinationFilePath { get; set; }

        [Element("Width")]
        public int? Width { get; set; }

        [Element("Height")]
        public int? Height { get; set; }

        #endregion
        #region Business Logic

        protected Zen.Zencoder GetNewZencoder()
        {
            return new Zen.Zencoder(AccessKey, new Uri(BaseUrl));
        }

        #endregion
    }
}
