using CHAOS.Octopus.Plugins.Transcoding.Zencoder;
using Geckon.Events;
using Geckon.Octopus.Plugin.Interface;
using NUnit.Framework;

namespace Geckon.Octopus.Plugins.Transcoding.Zencoder.Test
{
    [TestFixture]
    public class CutStillPluginTest 
    {
        [Test]
        public void Should_Create_Thumbnail()
        {
            var plugin = new CutStillPlugin()
                             {
                                 AccessKey = "aa86ad3b004917c8fe52b54fc38b09dd",
                                 Label = "somelabel",
                                 DestinationFilePath = "s3://chaosdata/hobit.jpg",
                                 Width = 480,
                                 Height = 320,
                                 SourceFilePath = "s3://chaosdata/hobit.mp4",
                                 BaseUrl = "https://app.zencoder.com/api/v2/",
                                 Number = 1
                             };

            plugin.ExecuteFailed += delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };
            plugin.OperationProgressChanged += delegate(object sender, ObjectProgressEventArgs<IPlugin> eventArgs){System.Console.WriteLine(eventArgs.NewProgress);};
            plugin.BeginExecute();
        }
    }
}
