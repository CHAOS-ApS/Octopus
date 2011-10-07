using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.AWS.S3
{
    public class S3CopyPlugin : APluginExtended
    {
        #region Properties

        protected readonly PluginPropertyFilePath _SourceFilePath;
        protected readonly PluginPropertyFilePath _DestinationFilePath;

        [Element("SourceFilePath")]
        public string SourceFilePath
        {
            get { return _SourceFilePath.Value; }
            set
            {
                _SourceFilePath.SetValueIfPropertiesAreEditable(value);
            }
        }

        [Element("DestinationFilePath")]
        public string DestinationFilePath
        {
            get { return _DestinationFilePath.Value; }
            set
            {
                _DestinationFilePath.SetValueIfPropertiesAreEditable(value);
            }
        }

        [Element("BucketName")]
        public string BucketName { get; set; }

        [Element("AWSAccessKey")]
        public string AWSAccessKey { get; set; }

        [Element("AWSSecretKey")]
        public string AWSSecretKey { get; set; }

        [Element("ContentType")]
        public string ContentType { get; set; }

        #endregion
        #region Constructors
        public S3CopyPlugin()
        {
            _DestinationFilePath = new PluginPropertyFilePath("DestinationFilePath", this);
            _SourceFilePath      = new PluginPropertyFilePath("SourceFilePath", this);
        }

        #endregion
        #region Business Logic
        protected override void Execute()
        {
            base.Execute();

            using( AmazonS3   S3Client   = AWSClientFactory.CreateAmazonS3Client( AWSAccessKey, AWSSecretKey ) )
            using( FileStream fileStream = GetStream() )
            {
                PutObjectRequest request = new PutObjectRequest();

                request.AutoCloseStream = false;
                request.BucketName      = BucketName;
                request.InputStream     = fileStream;
                request.Key             = DestinationFilePath.Replace("\\","/").TrimStart( '/' );
                request.ContentType     = ContentType; //video/mp4
                request.CannedACL       = S3CannedACL.PublicRead;
                request.Timeout         = 24 * 60 * 60 * 1000;

                request.PutObjectProgressEvent += ProgressEvent;

                for( int numberOfRetries = 60; numberOfRetries > 0; numberOfRetries-- )
                {
                    try
                    {
                        PutObjectResponse response = S3Client.PutObject( request );
                        break;
                    }
                    catch( Exception )
                    {
                        if( numberOfRetries - 1 == 0 )
                            throw;

                        System.Threading.Thread.Sleep( 1000 );
                    }
                }
            }
        }

        private FileStream GetStream()
        {
            for( int numberOfRetries = 60; numberOfRetries > 0; numberOfRetries-- )
            {
                try
                {
                    return new FileStream(Path.GetFullPath(SourceFilePath), FileMode.Open);
                }
                catch( Exception )
                {
                    System.Threading.Thread.Sleep( 1000 );
                    
                    continue;
                }
            }

            return new FileStream(Path.GetFullPath(SourceFilePath), FileMode.Open);
        }

        private void ProgressEvent(object sender, PutObjectProgressArgs args)
        {
            OperationProgress = args.PercentDone / 100.0;
        }

        protected override void Rollback()
        {
            base.Rollback();

            using( AmazonS3 S3Client = AWSClientFactory.CreateAmazonS3Client( AWSAccessKey, AWSSecretKey ) )
            {
                DeleteObjectRequest request = new DeleteObjectRequest();
                
                request.BucketName = BucketName;
                request.Key        = DestinationFilePath;

                DeleteObjectResponse response = S3Client.DeleteObject( request );
            }
        }
        #endregion

    }
}
