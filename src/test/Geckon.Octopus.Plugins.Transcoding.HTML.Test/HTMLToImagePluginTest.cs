namespace Geckon.Octopus.Plugins.Transcoding.HTML.Test
{
	using System.Diagnostics;
	using NUnit.Framework;

	[TestFixture]
	public class HTMLToImagePluginTest //TODO: Add more unit tests
	{
		[Test]
		public void ShouldCaptureTestPage()
		{
			var plugin = new HTMLToImagePlugin
							{
								HTMLPath = "http://web.server00.geckon.com/Skolepakken/WEB/TaskTest/",
								DestinationPath = @"C:\test.png",
								Width = 500,
								Height = 400,
								ShouldOwerwriteExistingFile = true,
								ShouldWaitForJavascript = true
							};

			plugin.TotalProgressChanged += (sender, e) => Trace.WriteLine("Progress: " + e.NewProgress);

			plugin.BeginExecute();
			plugin.BeginCommit();
		}
	}
}