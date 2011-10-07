using System;
using System.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Carbon
{
	internal class CarbonVideoTranscoder
	{
		#region Fields

		private string _Host;
		private ushort _Port;
		private string _JobID;

		#endregion

		#region Business Logic

		public void Transcode(string host, ushort port, string carbonPresetGUID, string sourceFilePath, string destionationFilePath, uint maxWidth, uint maxHeight, uint minAudioBitRate, uint minVideoBitRate, ushort maxFrameRate)
		{
			_Host = host;
			_Port = port;
			
			var videoInformation = CarbonQueryVideoInformation.GetVideoDimension(_Host, _Port, sourceFilePath, minAudioBitRate, minVideoBitRate, maxFrameRate);

			videoInformation.ConstrainDimensionPreservingAspectRatio(maxWidth, maxHeight);

			var queryXml = CarbonQueryTranscodeVideo.CompileVideoEncodeJobXml(carbonPresetGUID, sourceFilePath, destionationFilePath, videoInformation);

			var reply = new XmlDocument();
			reply.LoadXml(CarbonQuery.Query(_Host, _Port, queryXml));

			if (reply.DocumentElement == null)
				throw new Exception("Carbon job response not valid");
			
			_JobID = reply.DocumentElement.Attributes["GUID"].InnerText;
		}

		public CarbonJobStatus GetStatus()
		{
			if (_JobID == null)
				throw new Exception("Job not started yet");
			
			var jobStatus = new XmlDocument();

			jobStatus.LoadXml(CarbonQuery.GetCarbonJobStatusXml(_Host, _Port, _JobID));

			var jobInfoNode = jobStatus.SelectSingleNode("//JobInfo");

			var progress = 0d;

			if(jobInfoNode.Attributes["Progress.DWD"] != null)
				progress = double.Parse(jobInfoNode.Attributes["Progress.DWD"].Value) / 100;

			if (jobInfoNode.Attributes["Status"] != null && jobInfoNode.Attributes["Status"].Value == "ERROR")
				return new CarbonJobStatus(_JobID, progress, true);

			return new CarbonJobStatus(_JobID, progress, false);
		}

		#endregion
	}
}