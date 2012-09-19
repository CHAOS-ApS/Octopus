using System;
using CHAOS.Octopus.Plugins.Transcoding.Zencoder;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;
using NUnit.Framework;
using Zen = Zencoder;

namespace Geckon.Octopus.Plugins.Transcoding.Zencoder.Test
{
    [TestFixture]
    public class ZencoderPluginTest
    {
        [Test]
        public void Should_Execute_ZencoderPlugin()
        {
            var plugin = new ZencoderPlugin
                             {
                                 AccessKey = "aa86ad3b004917c8fe52b54fc38b09dd",
                                 Label = "somelabel",
                                 DestinationFilePath = "s3://chaosdata/outtest.mp4",
                                 Width = 480,
                                 Height = 320,
                                 SourceFilePath = "s3://chaosdata/test.mp4",
                                 BaseURL = "https://app.zencoder.com/api/v2/"
                             };

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };
            plugin.OperationProgressChanged += delegate(object sender, ObjectProgressEventArgs<IPlugin> eventArgs){System.Console.WriteLine(eventArgs.NewProgress);};
            plugin.BeginExecute();
        }
    }
}
