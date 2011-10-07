using System;
using System.IO;
using System.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Carbon
{
	internal static class CarbonQueryVideoInformation
	{
		public static CarbonVideoInformation GetVideoDimension(string host, ushort port, string filename, uint minAudioBitRate, uint minVideoBitRate, ushort maxFrameRate)
		{
			var requestXmlString =
				CompileVideoInformationRequestXmlString(filename);

			var responseString = CarbonQuery.Query(host, port, requestXmlString);

			var responseXml = new XmlDocument();
			responseXml.LoadXml(responseString);

			var jobResultNode = responseXml.SelectSingleNode(@"//JobEvaluateResult");
			if (jobResultNode.Attributes["LoadJobError"] != null)
				throw new Exception(
					"Error getting video dimension: " +
					jobResultNode.Attributes["LoadJobError"].Value
					);

			var publicDescriptionNode = responseXml.SelectSingleNode(@"//PublicDescription");

			var videoNode = publicDescriptionNode.SelectSingleNode(@"Video");

			var fileSizeBytes = new FileInfo(filename).Length;

			var frameDurationHertz = long.Parse(videoNode.Attributes["FrameDuration.QWD"].Value);
			var secondsPerFrame = (decimal)frameDurationHertz / 27000000;

			var fileDurationHertz = long.Parse(publicDescriptionNode.Attributes["Duration.QWD"].Value);

			var framerate = (ushort)Math.Round(
				1 / ((decimal)frameDurationHertz / 27000000)
				);

			var fileDurationSeconds =
				((decimal)fileDurationHertz / frameDurationHertz) *
				secondsPerFrame;

			var totalBitRate = 8 * (((decimal)fileSizeBytes / 1000) / fileDurationSeconds);

			var audioBitRate = (uint)Math.Round(
				totalBitRate * ((decimal)minAudioBitRate / minVideoBitRate)
				);
			var videoBitRate = (uint)Math.Round(totalBitRate - audioBitRate);

			if (videoBitRate < minVideoBitRate)
				videoBitRate = minVideoBitRate;
			if (audioBitRate < minAudioBitRate)
				audioBitRate = minAudioBitRate;

			if (framerate > maxFrameRate)
				framerate = maxFrameRate;

			return new CarbonVideoInformation(
				uint.Parse(videoNode.Attributes["Size_X.DWD"].Value),
				uint.Parse(videoNode.Attributes["Size_Y.DWD"].Value),
				audioBitRate,
				videoBitRate,
				framerate
				);
		}

		private static string CompileVideoInformationRequestXmlString(string filename)
		{
			var xml = new XmlDocument();

			var rootElement = xml.CreateElement("cnpsXML");

			rootElement.SetAttribute("CarbonAPIVer", "1.2");
			rootElement.SetAttribute("TaskType", "JobEvaluate");

			var sourcesElement = xml.CreateElement("Sources");

			var destinationsElement = xml.CreateElement("Destinations");

			rootElement.AppendChild(sourcesElement);
			rootElement.AppendChild(destinationsElement);

			var inputFileElement = xml.CreateElement("Module_0");
			inputFileElement.SetAttribute("Filename", filename);


			sourcesElement.AppendChild(inputFileElement);

			xml.AppendChild(rootElement);

			return
				"<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>" +
				xml.OuterXml;
		}
	}
}