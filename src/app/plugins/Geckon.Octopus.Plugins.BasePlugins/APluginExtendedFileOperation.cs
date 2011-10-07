using System;
using System.IO;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public abstract class APluginExtendedFileOperation : APluginExtendedTemporaryFile
	{
		#region Fields

		protected const uint ROLLBACK_LEVEL_FILE_LOCATIONS_VALIDATED = ROLLBACK_LEVEL_PROPERTIES_CHECKED + 1000;
		protected const uint ROLLBACK_LEVEL_FILE_OPERATION_BEGUN = ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED + 1000;
		protected const uint ROLLBACK_LEVEL_FILE_OPERATION_ENDED = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN + 10000;

		protected readonly PluginPropertyFilePath _SourceFilePath;
		protected readonly PluginPropertyFilePath _DestinationFilePath;
		protected readonly PluginProperty<bool> _ShouldOwerwriteExistingFile;

		#endregion
		#region Properties

		[Element("SourceFilePath")]
		public string SourceFilePath
		{
			get { return _SourceFilePath.Value; }
			set
			{
				_SourceFilePath.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("DestinationFilePath")]
		public string DestinationFilePath
		{
			get { return _DestinationFilePath.Value; }
			set
			{
				_DestinationFilePath.SetValueIfPropertiesAreEditable(value);
			}
		}

		[Element("ShouldOwerwriteExistingFile")]
		public bool ShouldOwerwriteExistingFile
		{
			get { return _ShouldOwerwriteExistingFile.Value; }
			set
			{
				_ShouldOwerwriteExistingFile.SetValueIfPropertiesAreEditable(value);
			}
		}

		#endregion
		#region Constructors

		protected APluginExtendedFileOperation()
		{
			_SourceFilePath = new PluginPropertyFilePath("SourceFilePath", this);
			_DestinationFilePath = new PluginPropertyFilePath("DestinationFilePath", this);
			_ShouldOwerwriteExistingFile = new PluginProperty<bool>("ShouldOwerwriteExistingFile", this, false);
		}

		#endregion
		#region Business Logic

		protected override void Execute()
		{
			_SourceFilePath.ValidatePropertyIsSet();
			_DestinationFilePath.ValidatePropertyIsSet();

			RollbackLevel = ROLLBACK_LEVEL_PROPERTIES_CHECKED;

			_SourceFilePath.ValidateFileExist();

			_DestinationFilePath.ValidateDirectoryExist();

			RollbackLevel = ROLLBACK_LEVEL_FILE_LOCATIONS_VALIDATED;

			ClearDestionationPath();

			RollbackLevel = ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED;
		}

		protected override void Commit()
		{
			ValidateTemporaryFilePath();
			
			base.Commit();
		}

		protected override void Rollback()
		{
			if (RollbackLevel < ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED)
				return;
			
			_DestinationFilePath.ValidatePropertyIsSet();

			ValidateTemporaryFilePath();
		}

		protected void ValidateTemporaryFilePath()
		{
			if (!_TemporaryFilePath.IsSet)
				return;

			if (!ShouldOwerwriteExistingFile)
				throw new ArgumentException("TemporaryFilePath should not be set when ShouldOwerwriteExistingFile is False");

			_TemporaryFilePath.ValidateFileExist();
		}

		protected void ClearDestionationPath()
		{
			if (!ShouldOwerwriteExistingFile)
			{
				_DestinationFilePath.ValidateFileDoesNotExist();
				return;
			}

			RenameExistingFileIfExist();
		}

		protected void RenameExistingFile()
		{
			RenameExistingFile(_DestinationFilePath);
		}

		protected void RenameExistingFileIfExist()
		{
			RenameExistingFileIfExist(_DestinationFilePath);
		}

		protected void RestoreTemporaryFile()
		{
			RestoreTemporaryFile(_DestinationFilePath);
		}

		protected void RestoreTemporaryFileIfExist()
		{
			RestoreTemporaryFileIfExist(_DestinationFilePath);
		}

		protected void CreateUniqueFolderFromDestinationName(PluginPropertyDirectoryPath directoryPath)
		{
			directoryPath.ValidateDirectoryDoesNotExist();

			if (!directoryPath.IsSet)
			{
				directoryPath.Value = Path.Combine(Path.GetDirectoryName(DestinationFilePath), _DestinationFilePath.FileNameWithoutExtension + "_TempFolder");
				
				while (directoryPath.DoesDirectoryExist)
					directoryPath.Value = Path.Combine(Path.GetDirectoryName(DestinationFilePath), _DestinationFilePath.FileNameWithoutExtension + "_TempFolder" + Path.GetRandomFileName());
			}

			Directory.CreateDirectory(directoryPath.DirectoryPath);
		}

		#endregion
		#region Helper Functions

		protected string GetUniqueFilePathWithoutExtensionInDestinationDirectory()
		{
			if(!_DestinationFilePath.IsSet)
				throw new Exception("Destination not set");

			return GetUniqueFilePathWithoutExtension(_DestinationFilePath.FilePath);
		}

		#endregion
	}
}