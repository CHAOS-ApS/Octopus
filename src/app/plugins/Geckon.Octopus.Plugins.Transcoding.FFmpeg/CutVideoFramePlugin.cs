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

namespace Geckon.Octopus.Plugins.Transcoding.FFmpeg
{
	public class CutVideoFramePlugin : APluginExtendedFileOperation
	{
		#region Fields

		private readonly PluginPropertyFilePath _FFmpegFilePath;

		private readonly PluginPropertyString _VideoPosition;

		private readonly PluginPropertyValueType<uint> _Width;
		private readonly PluginPropertyValueType<uint> _Height;

		private readonly PluginPropertyValueType<bool> _ShouldKeepAspectRatio;

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

		[Element("VideoPosition")]
		public string VideoPosition
		{
			get { return _VideoPosition.Value; }
			set
			{
				TimeSpan.Parse(value); // Should throw a exception if it can not be parsed
				
				_VideoPosition.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("Width")]
		public uint Width
		{
			get { return _Width.Value; }
			set
			{
				_Width.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("Height")]
		public uint Height
		{
			get { return _Height.Value; }
			set
			{
				_Height.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("ShouldKeepAspectRatio")]
		public bool ShouldKeepAspectRatio
		{
			get { return _ShouldKeepAspectRatio.Value; }
			set
			{
				_ShouldKeepAspectRatio.SetValueIfPropertiesAreEditable(value);
			}
		}

		#endregion
		#region Constructors

		public CutVideoFramePlugin()
		{
			_FFmpegFilePath = new PluginPropertyFilePath("FFmpegFilePath", this, "ffmpeg.exe");

			_VideoPosition = new PluginPropertyString("Position", this, "", true);

			_Width = new PluginPropertyValueType<uint>("Width", this, 0);
			_Height = new PluginPropertyValueType<uint>("Height", this, 0);

			_ShouldKeepAspectRatio = new PluginPropertyValueType<bool>("ShouldKeepAspectRatio", this, true);
		}

		#endregion
		#region Business Logic

		protected override void Execute()
		{
			if (_FFmpegFilePath.IsSet)
				_FFmpegFilePath.ValidateFileExist();

			_VideoPosition.ValidatePropertyIsSet();

			if (_Width.IsSet != _Height.IsSet || (_Width.Value == 0 ^ _Height.Value == 0))
				throw new Exception("Both or none of Width and Height must be set");

			base.Execute();

			RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;

			if(ShouldKeepAspectRatio)
				AdjustVideoSize();

			CutFrame();

			if(!_DestinationFilePath.DoesFileExist)
				throw new IOException("No file was created");

			RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_ENDED;
		}

		private void AdjustVideoSize()
		{
			var inputFile = new InputFileOptions();
			inputFile.FilePath = Path.GetFullPath(SourceFilePath);

			using (var wrapper = new FFmpegWrapper())
			{
				wrapper.FFmpegExecutablePath = Path.GetFullPath(FFmpegFilePath);

				wrapper.AddInputFileOptions(inputFile);

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

				if (sizeData.Count() == 0)
					return;

				var firstSizeData = sizeData.First();

				var aspect = firstSizeData.DisplayAspectRatio ?? firstSizeData.Size.Width / (double)firstSizeData.Size.Height; //Use DAR if available, else use video resolution

				if (Width / aspect < Height)
					_Height.Value = (uint)(Math.Round(Width / aspect / 2) * 2); //Make sure size is dividable by two
				else
					_Width.Value = (uint)(Math.Round(Height * aspect / 2) * 2); //Make sure size is dividable by two
			}
		}

		private void CutFrame()
		{
			var inputFile = new InputFileOptions();
			inputFile.FilePath = Path.GetFullPath(SourceFilePath);

			var outputFile = new OutputFileOptions();

			if (_Width.IsSet && _Width.Value != 0)
				GeckonPresets.ApplyCutVideoFramePreset(inputFile, outputFile, TimeSpan.Parse(_VideoPosition.Value), Width, Height);
			else
				GeckonPresets.ApplyCutVideoFramePreset(inputFile, outputFile, TimeSpan.Parse(_VideoPosition.Value));

			outputFile.FilePath = Path.GetFullPath(DestinationFilePath);

			using (var wrapper = new FFmpegWrapper())
			{
				wrapper.FFmpegExecutablePath = Path.GetFullPath(FFmpegFilePath);

				wrapper.AddInputFileOptions(inputFile);
				wrapper.OutputFileOptions = outputFile;

				wrapper.ProgressDataChanged += ProgressDataChanged;

				wrapper.RawOutputOccurred += (sender, e) => Console.WriteLine(e.EventObject);

				wrapper.Execute();

				wrapper.ProgressDataChanged -= ProgressDataChanged;
			}
		}

		private void ProgressDataChanged(object sender, ObjectEventArgs<ProgressData> eventArgs)
		{
			OperationProgress = (sender as IFFmpegWrapper).CompletionProgress / 2 + 0.5;
		}

		protected override void Rollback()
		{
			base.Rollback();

			if (RollbackLevel >= ROLLBACK_LEVEL_FILE_OPERATION_BEGUN && _DestinationFilePath.DoesFileExist)
				File.Delete(_DestinationFilePath.FilePath);

			if (RollbackLevel >= ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED)
				RestoreTemporaryFileIfExist();
		}

		#endregion
	}
}