using System;
using System.IO;
using System.Text.RegularExpressions;
using Amazon;
using Amazon.S3.Model;
using Geckon.Graphics;
using Geckon.Logging;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Image.S3
{
    public class ImageResizePlugin : APlugin
    {
        #region Properties

        [Element("AWSAccessKey")]
        public string AwsAccessKey { get; set; }

        [Element("AWSSecretKey")]
        public string AwsSecretKey { get; set; }

        [Element("DestinationFilePath")]
        public string DestinationFilePath { get; set; }

        [Element("SourceFilePath")]
        public string SourceFilePath { get; set; }

        [Element("TempSourceFilepath")]
        public string TempSourceFilepath { get; set; }

        [Element("TempDestinationFilepath")]
        public string TempDestinationFilepath { get; set; }

        [Element("Width")]
        public int Width { get; set; }

        [Element("Height")]
        public int Height { get; set; }

        [Element("Quality")]
        public int Quality { get; set; }

        #endregion
        #region Business Logic

        protected override void Execute()
        {
            base.Execute();
        //s3-eu-west-1.amazonaws.com/chaosdata/hobit.mp4
            OperationProgress = 0.1;
            var sourceS3Settings        = GetS3Settings(SourceFilePath); OperationProgress = 0.2;
            var destinationS3Settings   = GetS3Settings(DestinationFilePath); OperationProgress = 0.3;
            
            TempSourceFilepath      = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(DestinationFilePath)); OperationProgress = 0.4;
            TempDestinationFilepath = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(DestinationFilePath)); OperationProgress = 0.5;

            using (var s3 = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretKey, GetRegion(sourceS3Settings.Region)))
            {
                OperationProgress = 0.6;
                var request = new Amazon.S3.Model.GetObjectRequest
                                  {
                                      BucketName = sourceS3Settings.BucketName,
                                      Key = sourceS3Settings.Key
                                  }; OperationProgress = 0.7;

                using (var stream = s3.GetObject(request).ResponseStream)
                using (var file   = new FileStream(TempSourceFilepath, FileMode.Create))
                {
                    OperationProgress = 0.8;
                    stream.CopyTo(file);
                }
                OperationProgress = 0.9;
            }

            using (var imaging = new Imaging(TempSourceFilepath))
            {
                imaging.Resize(Width, Height, FillTypes.scale, Quality, TempDestinationFilepath);
            }
            
            OperationProgress = 0.9;
            using (var s3 = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretKey, GetRegion(destinationS3Settings.Region)))
            {
                OperationProgress = 0.10;
                var request = new Amazon.S3.Model.PutObjectRequest
                                  {
                                      BucketName      = destinationS3Settings.BucketName,
                                      Key             = destinationS3Settings.Key,
                                      FilePath        = TempDestinationFilepath,
                                      AutoCloseStream = true,
                                      Timeout         = 30 * 60 * 60,
                                      CannedACL       = S3CannedACL.PublicRead
                                  };
                OperationProgress = 0.11;
                s3.PutObject(request);
            }
        }

        protected override void Commit()
        {
            base.Commit();

            if (File.Exists(TempSourceFilepath))
                File.Delete(TempSourceFilepath);

            if (File.Exists(TempDestinationFilepath))
                File.Delete(TempDestinationFilepath);
        }

        protected override void Rollback()
        {
            base.Rollback();

            if (File.Exists(TempSourceFilepath))
                File.Delete(TempSourceFilepath);

            if (File.Exists(TempDestinationFilepath))
                File.Delete(TempDestinationFilepath);
        }

        private static S3Settings GetS3Settings(string s3Path)
        {
            var regexMatch = Regex.Match(s3Path, @"^http[s]*://(.+)\.amazonaws\.com/(.+?)/(.+)$");

            return new S3Settings
                       {
                           Region     = regexMatch.Groups[1].Value,
                           BucketName = regexMatch.Groups[2].Value,
                           Key        = regexMatch.Groups[3].Value
                       };
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

    internal class S3Settings
    {
        #region properties

        public string Region { get; set; }
        public string BucketName { get; set; }
        public string Key { get; set; }

        #endregion
    }
}
