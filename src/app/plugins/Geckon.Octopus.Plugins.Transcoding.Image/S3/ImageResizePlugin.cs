using System;
using System.IO;
using System.Text.RegularExpressions;
using Amazon;
using Geckon.Graphics;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Image.S3
{
    public class ImageResizePlugin : APluginExtendedFileOperation
    {
        #region Properties

        [Element("AWSAccessKey")]
        public string AwsAccessKey { get; set; }

        [Element("AWSSecretKey")]
        public string AwsSecretKey { get; set; }

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
            //  s3://s3-eu-west-1.amazonaws.com/chaosdata/hobit.mp4

            var sourceS3Settings        = GetS3Settings(SourceFilePath);
            var destinationS3Settings   = GetS3Settings(DestinationFilePath);

            TempSourceFilepath      = Path.GetRandomFileName();
            TempDestinationFilepath = Path.GetRandomFileName();
   
            using (var s3 = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretKey, GetRegion(sourceS3Settings.Region)))
            {
                var request = new Amazon.S3.Model.GetObjectRequest
                                  {
                                      BucketName = sourceS3Settings.BucketName,
                                      Key        = sourceS3Settings.Key
                                  };

                using (var stream = s3.GetObject(request).ResponseStream)
                using (var file   = new FileStream(TempSourceFilepath, FileMode.Create))
                {
                    stream.CopyTo(file);
                }
            }

            RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;

            new Imaging(TempSourceFilepath).Resize(Width, Height, FillTypes.scale, Quality, TempDestinationFilepath);

            RollbackLevel = ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED;

            using (var s3 = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretKey, GetRegion(destinationS3Settings.Region)))
            {
                var request = new Amazon.S3.Model.PutObjectRequest
                                  {
                                      BucketName      = destinationS3Settings.BucketName,
                                      Key             = destinationS3Settings.Key,
                                      FilePath        = TempDestinationFilepath,
                                      AutoCloseStream = true,
                                      Timeout         = 30*60*60
                                  };

                s3.PutObject(request);
            }

            RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_ENDED;
        }

        protected override void Commit()
        {
            base.Commit();

            if(File.Exists(TempSourceFilepath))
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
            var regexMatch = Regex.Match(s3Path, @"^s3://(.+)\.amazonaws\.com/(.+)/(.+)$");

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
