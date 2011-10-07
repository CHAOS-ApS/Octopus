using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Geckon.Octopus.Controller.Core;
using Geckon.Octopus.Controller.Interface;
using Geckon.Octopus.Data;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.TestUtilities;
using NUnit.Framework;
using Job = Geckon.Octopus.Controller.Core.Job;

namespace Geckon.Octopus.Plugins.Transcoding.FFmpeg.Test
{
	[TestFixture]
	public class TranscodeTwoPassh264PluginTest
	{
		#region SetUp/TearDown

		[SetUp]
		public void SetUp()
		{
			PluginLoader.Clear();
		}

		[TearDown]
		public void TearDown()
		{
			PluginLoader.Clear();
		}

		#endregion

		[Test]
		public void TestTranscode()
		{
			var plugin = CreatePlugin();

			plugin.TotalProgressChanged += (sender, e) => Trace.WriteLine("Progress: " + e.NewProgress);

			plugin.BeginExecute();

			Trace.WriteLine(plugin.FFmpegArguments);
			Trace.WriteLine(plugin.FFmpegOutput);

			plugin.BeginCommit();
		}

		[Test]
		public void TestOctopusTranscode()
		{
			using (DatabaseDataContext db = new DatabaseDataContext())
			{
				db.Test_InsertDemoData(Regex.Replace(Environment.CurrentDirectory, @"src\\(?:demo|test)\\(?:plugins\\)?[\w.\-]+\\bin\\(?:Debug|Release)$", "bin\\plugins\\"));

				using (IControllerEngine controllerEngine = new ControllerEngine(true))
				{
					var job = new Job();
					var step = new Step();

					step.Add(CreatePlugin());
					job.Add(step);

					db.Job_Insert( job.StatusID, job.JobXML.ToString());

					Timing.WaitUntil(() => db.Job_GetUnfinishedJobs().ToList().Count == 1, 10 * 1000, "Wait til job has been added");
					Timing.WaitUntil(() => db.Job_GetUnfinishedJobs().ToList().Count == 0, 120 * 1000, "Wait til job has completed");
				}
			}
		}

		private TranscodeTwoPassh264Plugin CreatePlugin()
		{
			var plugin = new TranscodeTwoPassh264Plugin();

			plugin.DestinationFilePath = @".\Test.mp4";
			plugin.SourceFilePath = @"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv";

			//plugin.SourceFilePath = @"D:\SONY_DVD_RECORDER_VOLUME\VIDEO_TS\VTS_01_1.VOB";

			//plugin.SourceFilePath = @"D:\test.avi";
			//plugin.SourceFilePath = @"D:\test.vob";
			//plugin.SourceFilePath = @"D:\VTS_01_1.VOB";


			plugin.FFmpegFilePath = @"..\..\..\..\..\..\lib\ffmpeg.exe";
			plugin.VideoBitrate = 1000000;
			plugin.VideoWidth = 500;
			plugin.VideoHeight = 280;
			plugin.AudioBitrate = 64000;
		    plugin.ShouldOwerwriteExistingFile = true;

			return plugin;
		}

		private CutVideoFramePlugin CreatePlugin2()
		{
			var plugin = new CutVideoFramePlugin();

			plugin.DestinationFilePath = @".\Test.png";
			plugin.SourceFilePath = @"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv";

			plugin.FFmpegFilePath = @"..\..\..\..\..\..\lib\ffmpeg.exe";
			plugin.VideoPosition = new TimeSpan(0, 0, 0, 5).ToString();
			plugin.Width = 480;
			plugin.Height = 270;

			return plugin;
		}
	}
}