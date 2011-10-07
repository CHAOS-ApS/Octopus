using System;
using System.IO;
using System.Linq;
using Geckon.Events;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Serialization.Xml;
using Geckon.Transcoding.FFMpeg;
using Geckon.Transcoding.FFMpeg.Data;
using Geckon.Transcoding.FFMpeg.FileOptions;
using Geckon.Transcoding.FFMpeg.FileOptions.Presets;
using Geckon.Transcoding.FFMpeg.Options.General;

namespace Geckon.Octopus.Plugins.Transcoding.FFmpeg
{
	using System.Text;
	using Geckon.Transcoding.FFMpeg.Options.Video;

	public class TranscodeTwoPassh264Plugin : APluginExtendedFileOperation
	{
		#region Fields

		private const uint ROLLBACK_LEVEL_TEMPORARY_DIRECTORY_CREATED = ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED + 500;
		private const uint ROLLBACK_LEVEL_TEMPORARY_DIRECTORY_DELETED = ROLLBACK_LEVEL_FILE_OPERATION_ENDED + 500;

		private readonly PluginPropertyFilePath _FFmpegFilePath;

		private readonly PluginPropertyDirectoryPath _TemporaryDirectoryPath;

		private readonly PluginPropertyValueType<uint> _AudioBitrate;

		private readonly PluginPropertyValueType<bool> _ShouldDeinterlaceVideo;
		private readonly PluginPropertyValueType<bool> _ShouldKeepVideoAspectRatio;
		

		private readonly PluginPropertyValueType<uint> _VideoWidth;
		private readonly PluginPropertyValueType<uint> _VideoHeight;
		private readonly PluginPropertyValueType<uint> _VideoBitrate;

		private readonly StringBuilder _FFmpegOutput;

		#endregion
		#region Properties

		[Element("FFmpegFilePath")]
		public string FFmpegFilePath
		{
			get { return _FFmpegFilePath.Value; }
			set
			{
				_FFmpegFilePath.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("TemporaryDirectoryPath")]
		public string TemporaryDirectoryPath
		{
			get { return _TemporaryDirectoryPath.Value; }
			set
			{
				_TemporaryDirectoryPath.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("AudioBitrate")]
		public uint AudioBitrate
		{
			get { return _AudioBitrate.Value; }
			set
			{
				_AudioBitrate.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("ShouldDeinterlaceVideo")]
		public bool ShouldDeinterlaceVideo
		{
			get { return _ShouldDeinterlaceVideo.Value; }
			set
			{
				_ShouldDeinterlaceVideo.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("ShouldKeepVideoAspectRatio")]
		public bool ShouldKeepVideoAspectRatio
		{
			get { return _ShouldKeepVideoAspectRatio.Value; }
			set
			{
				_ShouldKeepVideoAspectRatio.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("VideoWidth")]
		public uint VideoWidth
		{
			get { return _VideoWidth.Value; }
			set
			{
				_VideoWidth.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("VideoHeight")]
		public uint VideoHeight
		{
			get { return _VideoHeight.Value; }
			set
			{
				_VideoHeight.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("VideoBitrate")]
		public uint VideoBitrate
		{
			get { return _VideoBitrate.Value; }
			set
			{
				_VideoBitrate.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("FFmpegOutput")]
		public string FFmpegOutput
		{
			get { return _FFmpegOutput.ToString(); }
			set
			{ 
				if(value == null)
					return;
				
				_FFmpegOutput.Clear();
				_FFmpegOutput.Append(value);
			}
		}

		[Element("FFmpegArguments")]
		public string FFmpegArguments { get; set; }

		#endregion
		#region Construction
		
		public TranscodeTwoPassh264Plugin()
		{
			_FFmpegFilePath = new PluginPropertyFilePath("FFmpegFilePath", this);

			_TemporaryDirectoryPath = new PluginPropertyDirectoryPath("TemporaryDirectoryPath", this);

			_AudioBitrate = new PluginPropertyValueType<uint>("AudioBitrate", this);

			_ShouldDeinterlaceVideo = new PluginPropertyValueType<bool>("ShouldDeinterlaceVideo", this, false);
			_ShouldKeepVideoAspectRatio = new PluginPropertyValueType<bool>("ShouldKeepVideoAspectRatio", this, true);
			
			_VideoWidth = new PluginPropertyValueType<uint>("VideoWidth", this);
			_VideoHeight = new PluginPropertyValueType<uint>("VideoHeight", this);
			_VideoBitrate = new PluginPropertyValueType<uint>("VideoBitrate", this);

			_AudioBitrate.AddIsInRangeTest(value => value > 0);

			_VideoWidth.AddIsInRangeTest(value => value >= 0);
			_VideoHeight.AddIsInRangeTest(value => value >= 0);
			_VideoBitrate.AddIsInRangeTest(value => value > 0);

			_FFmpegOutput = new StringBuilder();
		}

		#endregion
		#region Business Logic

		protected override void Execute()
		{
			if(_FFmpegFilePath.IsSet)
				_FFmpegFilePath.ValidateFileExist();

			_AudioBitrate.ValidatePropertyIsSet();
			_VideoBitrate.ValidatePropertyIsSet();

			if(_VideoWidth.IsSet != _VideoHeight.IsSet || (_VideoWidth.Value == 0 ^ _VideoHeight.Value == 0))
				throw new Exception("Both or none of VideoWidth and ViewHeight must be set");

			base.Execute();

			CreateUniqueFolderFromDestinationName(_TemporaryDirectoryPath);

			RollbackLevel = ROLLBACK_LEVEL_TEMPORARY_DIRECTORY_CREATED;

			if (ShouldKeepVideoAspectRatio)
				AdjustVideoSize();
			
			RunFirstPass();

			RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;

			RunSecondPass();

			if(!_DestinationFilePath.DoesFileExist)
				throw new FileNotFoundException("Output video file was not created", DestinationFilePath);

			RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_ENDED;

			Directory.Delete(_TemporaryDirectoryPath.DirectoryPath, true);

			RollbackLevel = ROLLBACK_LEVEL_TEMPORARY_DIRECTORY_DELETED;
		}

		private void AdjustVideoSize()
		{
			var inputFile = new InputFileOptions();
			inputFile.FilePath = Path.GetFullPath(SourceFilePath);

			using (var wrapper = new FFmpegWrapper())
			{
				wrapper.FFmpegExecutablePath = Path.GetFullPath(FFmpegFilePath);

				wrapper.AddInputFileOptions(inputFile);

				wrapper.WorkingDirectory = TemporaryDirectoryPath;

				wrapper.UnparsedOutputOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject);
				wrapper.WarningOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject.Warning);
				wrapper.ErrorOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject.ToString());

				wrapper.Execute();

				if (wrapper.MediaData.Inputs.Count < 1)
					return;

				StreamData stream;

				try
				{
					stream = wrapper.MediaData.Inputs[0].Streams.First(s => s.Value.Type == StreamType.Video).Value;
				}
				catch (InvalidOperationException)
				{
					return; //No video stream found, so aspectratio can be determined
				}
				
				var sizeData = stream.ParsedData.OfType<StreamSizeData>();

				if(sizeData.Count() == 0)
					return;

				var firstSizeData = sizeData.First();

				var aspect = firstSizeData.DisplayAspectRatio ?? firstSizeData.Size.Width / (double)firstSizeData.Size.Height; //Use DAR if available, else use video resolution

				if (VideoWidth / aspect < VideoHeight)
					_VideoHeight.Value = (uint)(Math.Round(VideoWidth / aspect / 2) * 2); //Make sure size is dividable by two
				else
					_VideoWidth.Value = (uint)(Math.Round(VideoHeight * aspect / 2) * 2); //Make sure size is dividable by two
			}
		}

		private void RunFirstPass()
		{
			var inputFile = new InputFileOptions();
			inputFile.FilePath = Path.GetFullPath(SourceFilePath);

			var outputFile = new OutputFileOptions();

			if (ShouldDeinterlaceVideo)
				outputFile.Add(new DeinterlaceVideoOption());

			if(_VideoWidth.IsSet && _VideoWidth.Value != 0)
				GeckonPresets.ApplyHighQualityFirstPassPreset(outputFile, VideoBitrate, VideoWidth, VideoHeight);
			else
				GeckonPresets.ApplyHighQualityFirstPassPreset(outputFile, VideoBitrate);

			outputFile.Add(new ThreadsOption(ThreadsOption.AUTOMATICALLY_SELECT_NUMBER_OF_THREADS));

			using (var wrapper = new FFmpegWrapper())
			{
				wrapper.FFmpegExecutablePath = Path.GetFullPath(FFmpegFilePath);

				wrapper.AddInputFileOptions(inputFile);
				wrapper.OutputFileOptions = outputFile;

				wrapper.WorkingDirectory = TemporaryDirectoryPath;

				wrapper.ProgressDataChanged += FirstPassProgressDataChanged;

				wrapper.UnparsedOutputOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject);
				wrapper.WarningOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject.Warning);
				wrapper.ErrorOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject.ToString());

				wrapper.Execute();

				FFmpegArguments = wrapper.FFmpegArguments;

				wrapper.ProgressDataChanged -= FirstPassProgressDataChanged;
			}
		}

		private void RunSecondPass()
		{
			var inputFile = new InputFileOptions();
			inputFile.FilePath = Path.GetFullPath(SourceFilePath);

			var outputFile = new OutputFileOptions();

			if (ShouldDeinterlaceVideo)
				outputFile.Add(new DeinterlaceVideoOption());

			if (_VideoWidth.IsSet && _VideoWidth.Value != 0)
				GeckonPresets.ApplyHighQualitySecondPassPreset(outputFile, VideoBitrate, AudioBitrate, VideoWidth, VideoHeight);
			else
				GeckonPresets.ApplyHighQualitySecondPassPreset(outputFile, VideoBitrate, AudioBitrate);

			outputFile.FilePath = Path.GetFullPath(DestinationFilePath);

			outputFile.Add(new ThreadsOption(ThreadsOption.AUTOMATICALLY_SELECT_NUMBER_OF_THREADS));

			using (var wrapper = new FFmpegWrapper())
			{
				wrapper.FFmpegExecutablePath = Path.GetFullPath(FFmpegFilePath);

				wrapper.AddInputFileOptions(inputFile);
				wrapper.OutputFileOptions = outputFile;

				wrapper.WorkingDirectory = TemporaryDirectoryPath;

				wrapper.ProgressDataChanged += SecondPassProgressDataChanged;

				wrapper.UnparsedOutputOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject);
				wrapper.WarningOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject.Warning);
				wrapper.ErrorOccurred += (sender, args) => _FFmpegOutput.AppendLine(args.EventObject.ToString());

				wrapper.Execute();

				FFmpegArguments += "\n" + wrapper.FFmpegArguments;

				wrapper.ProgressDataChanged -= SecondPassProgressDataChanged;
			}
		}

		private void FirstPassProgressDataChanged(object sender, ObjectEventArgs<ProgressData> eventArgs)
		{
			OperationProgress = Math.Min(((IFFmpegWrapper) sender).CompletionProgress / 2, 0.5);
		}

		private void SecondPassProgressDataChanged(object sender, ObjectEventArgs<ProgressData> eventArgs)
		{
			OperationProgress = Math.Min(((IFFmpegWrapper) sender).CompletionProgress / 2 + 0.5, 1);
		}

		protected override void Rollback()
		{
			base.Rollback();

			if (RollbackLevel >= ROLLBACK_LEVEL_TEMPORARY_DIRECTORY_CREATED && RollbackLevel < ROLLBACK_LEVEL_TEMPORARY_DIRECTORY_DELETED)
			{
				_TemporaryDirectoryPath.ValidatePropertyIsSet();

				Directory.Delete(_TemporaryFilePath.DirectoryPath);
			}

			if (RollbackLevel >= ROLLBACK_LEVEL_FILE_OPERATION_BEGUN && _DestinationFilePath.DoesFileExist)
				File.Delete(_DestinationFilePath.FilePath);

			if (RollbackLevel >= ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED)
				RestoreTemporaryFileIfExist();
		}

		#endregion
	}
}