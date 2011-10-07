using NUnit.Framework;

namespace Geckon.Octopus.Plugins.Transcoding.Carbon.Test
{
	[TestFixture]
	public class CarbonTranscodePluginTest
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
		}

		#endregion

		/*[Test]
		public void ShouldTranscode()
		{
			var plugin = new CarbonTranscodePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, temporaryFilePath e) { throw e.Exception; };

			plugin.CarbonServerHost = "carbon-master.edumedia.dk";
			plugin.CarbonServerPort = 1120;
			plugin.CarbonPresetGUID = "CBB3DE66-CB6F-4032-8FCF-D9D0E9224C57";
			plugin.MaxWidth = 320;
			plugin.MaxHeight = 240;

			plugin.MinAudioBitRate = 128 * 1024;
			plugin.MinVideoBitRate = 1024 * 1024;
			plugin.MaxFrameRate = 15;

			plugin.SourceFilePath = @"C:\testVideo.mpg";
			plugin.DestinationFilePath = @"C:\CarbonTest.m4v";

			plugin.BeginExecute();
		}*/
	}
}
