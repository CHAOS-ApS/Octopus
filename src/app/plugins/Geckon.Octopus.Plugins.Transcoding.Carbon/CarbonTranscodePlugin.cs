using System;
using System.IO;
using System.Threading;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.Transcoding.Carbon
{
	public class CarbonTranscodePlugin : APluginExtendedFileOperation
	{
		#region Fields

		private const int CARBON_JOB_STATUS_UPDATE_INTERVAL = 3000;

		private readonly PluginPropertyString _CarbonServerHost;
		private readonly PluginPropertyValueType<ushort> _CarbonServerPort;

		private readonly PluginPropertyString _CarbonPresetGUID;

		private readonly PluginPropertyValueType<uint> _MaxWidth;
		private readonly PluginPropertyValueType<uint> _MaxHeight;

		private readonly PluginPropertyValueType<uint> _MinAudioBitRate;
		private readonly PluginPropertyValueType<uint> _MinVideoBitRate;

		private readonly PluginPropertyValueType<ushort> _MaxFrameRate;

		#endregion

		#region Properties

		[Element("CarbonServerHost")]
		public string CarbonServerHost
		{
			get { return _CarbonServerHost.Value; }
			set { _CarbonServerHost.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("CarbonServerPort")]
		public ushort CarbonServerPort
		{
			get { return _CarbonServerPort.Value; }
			set { _CarbonServerPort.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("CarbonPresetGUID")]
		public string CarbonPresetGUID
		{
			get { return _CarbonPresetGUID.Value; }
			set { _CarbonPresetGUID.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("MaxWidth")]
		public uint MaxWidth
		{
			get { return _MaxWidth.Value; }
			set { _MaxWidth.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("MaxHeight")]
		public uint MaxHeight
		{
			get { return _MaxHeight.Value; }
			set { _MaxHeight.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("MinAudioBitRate")]
		public uint MinAudioBitRate
		{
			get { return _MinAudioBitRate.Value; }
			set { _MinAudioBitRate.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("MinVideoBitRate")]
		public uint MinVideoBitRate
		{
			get { return _MinVideoBitRate.Value; }
			set { _MinVideoBitRate.SetValueIfPropertiesAreEditable(value); }
		}

		[Element("MaxFrameRate")]
		public ushort MaxFrameRate
		{
			get { return _MaxFrameRate.Value; }
			set { _MaxFrameRate.SetValueIfPropertiesAreEditable(value); }
		}

		#endregion

		#region Constructors

		public CarbonTranscodePlugin()
		{
			_CarbonServerHost = new PluginPropertyString("CarbonServerHost", this, "localhost");
			_CarbonServerPort = new PluginPropertyValueType<ushort>("CarbonServerPort", this, 1101);

			_CarbonPresetGUID = new PluginPropertyString("CarbonPresetGUID", this);

			_MaxWidth = new PluginPropertyValueType<uint>("MaxWidth", this);
			_MaxWidth.AddIsInRangeTest(maxWidth => maxWidth > 0);

			_MaxHeight = new PluginPropertyValueType<uint>("MaxHeight", this);
			_MaxHeight.AddIsInRangeTest(maxHeight => maxHeight > 0);

			_MinAudioBitRate = new PluginPropertyValueType<uint>("MinAudioBitRate", this);
			_MinAudioBitRate.AddIsInRangeTest(minAudioBitRate => minAudioBitRate > 0);

			_MinVideoBitRate = new PluginPropertyValueType<uint>("MinVideoBitRate", this);
			_MinVideoBitRate.AddIsInRangeTest(minVideoBitRate => minVideoBitRate > 0);

			_MaxFrameRate = new PluginPropertyValueType<ushort>("MaxFrameRate", this);
			_MaxFrameRate.AddIsInRangeTest(maxFrameRate => maxFrameRate > 0);
		}

		#endregion

		#region Business Logic

		protected override void Execute()
		{
			_CarbonServerHost.ValidatePropertyIsSet();
			_CarbonServerPort.ValidatePropertyIsSet();
			_CarbonPresetGUID.ValidatePropertyIsSet();
			_MaxWidth.ValidatePropertyIsSet();
			_MaxHeight.ValidatePropertyIsSet();
			_MinAudioBitRate.ValidatePropertyIsSet();
			_MinVideoBitRate.ValidatePropertyIsSet();
			_MaxFrameRate.ValidatePropertyIsSet();

			base.Execute();

			var uniqueName = GetUniqueFilePathWithoutExtensionInDestinationDirectory();

			try
			{
				using (var watcher = new FileSystemWatcher(Path.GetDirectoryName(DestinationFilePath)))
				{
					watcher.Filter = Path.GetFileName(DestinationFilePath);
					watcher.Created += WatcherCreated;
					watcher.EnableRaisingEvents = true;

					var transcoder = new CarbonVideoTranscoder();

					transcoder.Transcode(CarbonServerHost, CarbonServerPort, CarbonPresetGUID, _SourceFilePath.FilePath,
					                     uniqueName, MaxWidth, MaxHeight, MinAudioBitRate, MinVideoBitRate,
					                     MaxFrameRate);

					var jobStatus = transcoder.GetStatus();

					while (!jobStatus.HasEnded)
					{
						Thread.Sleep(CARBON_JOB_STATUS_UPDATE_INTERVAL);

						jobStatus = transcoder.GetStatus();

						OperationProgress = jobStatus.Progress;
					}

					if (jobStatus.HasFailed)
						throw new Exception("Carbon job failed");
				}

				
			}
			finally
			{
				FindUniqueFileAndMoveToDestination(uniqueName); //Rename the file no matter what so it can be deleted during rollback.
			}
		}

		private void WatcherCreated(object sender, FileSystemEventArgs e)
		{
			RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;
		}

		private void FindUniqueFileAndMoveToDestination(string uniqueFilePath)
		{
			var files = Directory.GetFiles(Path.GetDirectoryName(uniqueFilePath),
			                               Path.GetFileNameWithoutExtension(uniqueFilePath) + "*");

			if(files.Length == 0)
				return;

			if(files.Length > 1)
				throw new Exception("More than one match for unique file name path: " + uniqueFilePath);

			File.Move(files[0], _DestinationFilePath.FilePath);
		}

		protected override void Rollback()
		{
			base.Rollback();

			File.Delete(_DestinationFilePath.FilePath);

			RestoreTemporaryFileIfExist();
		}

		#endregion
	}
}