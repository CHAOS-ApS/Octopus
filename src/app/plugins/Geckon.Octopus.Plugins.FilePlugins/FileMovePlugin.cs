using System.IO;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugins.BasePlugins;

namespace Geckon.Octopus.Plugins.FilePlugins
{
	public class FileMovePlugin : APluginExtendedFileOperation
	{
		#region Fields

		private long _SourceFileSize;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Business Logic

		protected override void Execute()
		{
			base.Execute();

			MoveFile(_SourceFilePath.FilePath, _DestinationFilePath.FilePath);
		}

		private void MoveFile(string sourceFilePath, string destinationFilePath)
		{
			_SourceFileSize = new FileInfo(sourceFilePath).Length;

			using (var watcher = new FileSystemWatcher(Path.GetDirectoryName(destinationFilePath)))
			{
				watcher.Filter = Path.GetFileName(destinationFilePath);
				watcher.NotifyFilter = NotifyFilters.Size;
				watcher.EnableRaisingEvents = true;

				watcher.Created += WatcherCreated;

				if (_SourceFileSize != 0) //Avoid division by zero and it should not take any time to move an empty file.
					watcher.Changed += WatcherChanged;

				File.Move(sourceFilePath, destinationFilePath);
			}
		}

		private void WatcherCreated(object sender, FileSystemEventArgs e)
		{
			RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;
		}

		private void WatcherChanged(object sender, FileSystemEventArgs e)
		{
			var destinationFileSize = new FileInfo(e.FullPath).Length;

			OperationProgress = (double) destinationFileSize/_SourceFileSize;

			if (RollbackLevel != ROLLBACK_LEVEL_FILE_OPERATION_BEGUN)
				RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;
		}

		protected override void Rollback()
		{
			if(RollbackLevel < ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED)
				return;
			
			_SourceFilePath.ValidatePropertyIsSet();
			_SourceFilePath.ValidateFileDoesNotExist();

			base.Rollback();

			if (RollbackLevel >= ROLLBACK_LEVEL_FILE_OPERATION_BEGUN)
			{
				_DestinationFilePath.ValidateFileExist();
				MoveFile(_DestinationFilePath.FilePath, _SourceFilePath.FilePath);
			}
				

			if (RollbackLevel >= ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED)
				RestoreTemporaryFileIfExist();
		}

		#endregion
	}
}