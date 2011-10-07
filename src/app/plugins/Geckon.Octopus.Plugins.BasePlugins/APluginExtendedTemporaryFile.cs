using System.IO;
using Geckon.Serialization.Xml;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public abstract class APluginExtendedTemporaryFile : APluginExtended
	{
		#region Fields

		protected const uint ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED = ROLLBACK_LEVEL_PROPERTIES_CHECKED + 2000;

		protected readonly PluginPropertyFilePath _TemporaryFilePath;

		#endregion

		#region Properties

		[Element("TemporaryFilePath")]
		public string TemporaryFilePath
		{
			get { return _TemporaryFilePath.Value; }
			set
			{
				_TemporaryFilePath.SetValueIfPropertiesAreEditable(value);
			}
		}

		#endregion

		#region Constructors

		protected APluginExtendedTemporaryFile()
		{
			_TemporaryFilePath = new PluginPropertyFilePath("TemporaryFilePath", this);
		}

		#endregion
		
		#region Business Logic

		protected override void Commit()
		{
			if(!_TemporaryFilePath.IsSet)
				return;

			DeleteTemporaryFile();
		}

		protected void RenameExistingFile(PluginPropertyFilePath filePath)
		{
			RenameExistingFile(filePath, _TemporaryFilePath);
		}

		protected static void RenameExistingFile(PluginPropertyFilePath filePath, PluginPropertyFilePath temporaryFilePath)
		{
			filePath.ValidateFileExist();

			if (temporaryFilePath.IsSet)
			{
				temporaryFilePath.ValidateFileDoesNotExist();
			}
			else
			{
				temporaryFilePath.Value = Path.Combine(filePath.DirectoryPath, filePath.FileNameWithoutExtension + "_tmp" + filePath.FileExtension);

				while (!temporaryFilePath.IsSet || temporaryFilePath.DoesFileExist)
					temporaryFilePath.Value = Path.Combine(filePath.DirectoryPath, Path.Combine(filePath.DirectoryPath, filePath.FileNameWithoutExtension + "_tmp_" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + filePath.FileExtension));
			}
			
			File.Move(filePath.FilePath, temporaryFilePath.FilePath);
		}

		protected void RenameExistingFileIfExist(PluginPropertyFilePath filePath)
		{
			if(!filePath.DoesFileExist)
				return;
			
			RenameExistingFile(filePath);
		}

		protected void DeleteTemporaryFile()
		{
			_TemporaryFilePath.ValidateFileExist();

			File.Delete(_TemporaryFilePath.FilePath);
		}

		protected void RestoreTemporaryFileIfExist(PluginPropertyFilePath filePath)
		{
			if (!_TemporaryFilePath.DoesFileExist)
				return;
			
			RestoreTemporaryFile(filePath);
		}

		protected void RestoreTemporaryFile(PluginPropertyFilePath filePath)
		{
			filePath.ValidateFileDoesNotExist();
			_TemporaryFilePath.ValidateFileExist();

			File.Move(_TemporaryFilePath.FilePath, filePath.FilePath);
		}

		#endregion

		#region Helper Functions

		protected static string GetUniqueFilePathWithoutExtension(string path)
		{
			if (path == null)
				path = Path.GetTempPath();

			path = Path.GetDirectoryName(path);

			var uniqueFilePath = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
			var directory = new DirectoryInfo(path);

			while(directory.GetFiles(uniqueFilePath + "*").Length != 0)
				uniqueFilePath = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

			return Path.Combine(path, uniqueFilePath);
		}

		#endregion
	}
}