using System;
using System.Text.RegularExpressions;
using Geckon.Octopus.Plugin.Core;
using Geckon.Serialization.Xml;
using Zen = Zencoder;

namespace CHAOS.Octopus.Plugins.Transcoding.Zencoder
{
    public abstract class ZencoderBasePlugin : APlugin
    {
        #region Fields

      

        #endregion
        #region Properties

        [Element("AccessKey")]
        public string AccessKey { get; set; }

        [Element("Label")]
        public string Label { get; set; }

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

        protected static string ToZencoderUrl(S3Settings settings)
        {
            return string.Format("s3://{0}/{1}", settings.BucketName, settings.Key);
        }

        protected static string ToZencoderUrlWithoutFilename(S3Settings settings)
        {
            return string.Format("s3://{0}/{1}", settings.BucketName, settings.Key.Substring(0,settings.Key.LastIndexOf("/", StringComparison.Ordinal)));
        }

        protected static S3Settings GetS3Settings(string s3Path)
        {
            var regexMatch = Regex.Match(s3Path, @"^https://(.+)\.amazonaws\.com/(.+?)/(.+)$");

            return new S3Settings
            {
                Region     = regexMatch.Groups[1].Value,
                BucketName = regexMatch.Groups[2].Value,
                Key        = regexMatch.Groups[3].Value
            };
        }

        #endregion
    }
}
