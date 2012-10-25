using System;
using System.IO;
using System.Text.RegularExpressions;
using Amazon;
using Amazon.S3;
using Geckon.Graphics;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Image.S3
{
    public class ImageResizePlugin : APluginExtendedFileOperation
    {
        #region Properties

        [Element("AWSAccessKey")]
        public string AWSAccessKey { get; set; }

        [Element("AWSSecretKey")]
        public string AWSSecretKey { get; set; }

        

        #endregion
        #region Business Logic

        protected override void Execute()
        {
            base.Execute();
            //  s3://s3-eu-west-1.amazonaws.com/chaosdata/hobit.mp4

            var regexMatch = Regex.Match(SourceFilePath,@"^s3://(.+)\.amazonaws\.com/(.+)/(.+)$");

            var region                  = regexMatch.Groups[1].Value;
            var bucketName              = regexMatch.Groups[2].Value;
            var key                     = regexMatch.Groups[3].Value;
            var tempSourceFilepath      = Path.Combine(TemporaryFilePath, Path.GetRandomFileName());
            var tempDestinationFilepath = Path.Combine(TemporaryFilePath, Path.GetRandomFileName());

            using( var s3 = AWSClientFactory.CreateAmazonS3Client( AWSAccessKey, AWSSecretKey, GetRegion(region) ) )
            {
                var request = new Amazon.S3.Model.GetObjectRequest
                                  {
                                      BucketName = bucketName,
                                      Key        = key
                                  };

                using (var stream = s3.GetObject(request).ResponseStream)
                using (var file   = new FileStream(tempSourceFilepath, FileMode.Create))
                {
                    stream.CopyTo(file);
                }
            }

            new Imaging(tempSourceFilepath).Resize(10, 10, FillTypes.scale, 10, tempDestinationFilepath);


        }

        private static RegionEndpoint GetRegion(string region)
        {
            switch (region)
            {
                case "s3-eu-west-1":
                    return RegionEndpoint.EUWest1;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
