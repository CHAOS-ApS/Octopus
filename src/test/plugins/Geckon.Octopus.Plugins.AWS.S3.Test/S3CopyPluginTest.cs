using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Events;

namespace Geckon.Octopus.Plugins.AWS.S3.Test
{
    [TestFixture]
    public class S3CopyPluginTest
    {
        [Test]
		public void ShouldUploadImage()
		{
            var plugin = new S3CopyPlugin();
            plugin.ExecuteFailed +=
                delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

            plugin.AWSAccessKey = "AKIAJRSAPVB7R4JMCKCA";
            plugin.AWSSecretKey = "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";
            plugin.BucketName = "mcm";
            plugin.ContentType = "image/jpg";
            plugin.DestinationFilePath = GetDatePath() + "dublindock_jfk.jpg";
            plugin.SourceFilePath = @"dublindock_jfk.jpg";

            plugin.BeginExecute();
        }

        private string GetDatePath()
        {
            return DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
        }  

    }
}
