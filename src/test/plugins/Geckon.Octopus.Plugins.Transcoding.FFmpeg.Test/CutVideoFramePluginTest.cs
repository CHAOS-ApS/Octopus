using System;
using System.Diagnostics;
using Geckon.Serialization.Xml;
using NUnit.Framework;

namespace Geckon.Octopus.Plugins.Transcoding.FFmpeg.Test
{
	[TestFixture]
	public class CutVideoFramePluginTest
	{
		[Test]
		public void TestTranscode()
		{
			var plugin = CreatePlugin();

			plugin.TotalProgressChanged += (sender, e) => Trace.WriteLine("Progress: " + e.NewProgress);

			plugin.BeginExecute();
			plugin.BeginCommit();
		}

		private CutVideoFramePlugin CreatePlugin()
		{
			var plugin = new CutVideoFramePlugin();

			plugin.DestinationFilePath = @".\Test.png";
			//plugin.DestinationFilePath = @"\\web00\CHAOS\Temp\2011\03\29\OBJ-1270330_4f116107-c960-41fd-90ac-95d6e80cb22f_Wildlife.wmv.png";
			plugin.SourceFilePath = @"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv";
			//plugin.SourceFilePath = @"D:\Video\OBJ-1270055_6e1d7fbf-cba3-42b4-9eb2-58a7875e9b2e_Wildlife.wmv.mp4";
			//plugin.SourceFilePath = @"\\web00\CHAOS\Temp\2011\03\29\OBJ-1270330_4f116107-c960-41fd-90ac-95d6e80cb22f_Wildlife.wmv.mp4";
			//plugin.SourceFilePath = @"D:\Video\source_OBJ-1269938_afe20497-944c-4827-9765-995ed901a5b2_Borgen(0158-010016).mp4";

			plugin.ShouldOwerwriteExistingFile = true;

			//plugin.ShouldKeepAspectRatio = false;

			plugin.FFmpegFilePath = @"..\..\..\..\..\..\lib\ffmpeg.exe";
			//plugin.FFmpegFilePath = @"\\octopus00\c$\Geckon\Octopus\plugins\ffmpeg.exe";
			plugin.VideoPosition = new TimeSpan(0, 0, 0, 5).ToString();
			plugin.Width = 600;
			plugin.Height = 300;

			Console.WriteLine(XmlSerialize.ToXML(plugin).OuterXml);

			return plugin;
		}
	}
}