using System;
using System.IO;
using System.Security.AccessControl;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugins.BasePlugins;

namespace Geckon.Octopus.Plugins.FilePlugins
{
	public class FileCopyPlugin : APluginExtendedFileOperation
	{
		#region Properties

		#endregion
		#region Constructors

		#endregion
		#region Business Logic

		protected override void Execute()
		{
			base.Execute();

			CopyFile(_SourceFilePath.FilePath, _DestinationFilePath.FilePath);
		}

		private void CopyFile( string sourceFilePath, string destinationFilePath )
		{
            using( Stream source = new FileStream( sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
            {
                using( Stream destination = new FileStream( destinationFilePath, ShouldOwerwriteExistingFile ? FileMode.Create : FileMode.CreateNew , FileAccess.Write, FileShare.Read ) )
                {
                    RollbackLevel = ROLLBACK_LEVEL_FILE_OPERATION_BEGUN;

                    byte[] buffer = new byte[ 256 * 1024 ];
                    int    read;

                    while( ( read = source.Read( buffer, 0, buffer.Length ) ) > 0 )
                    {
                        destination.Write( buffer, 0, read );
                        OperationProgress = (double) destination.Length / source.Length;
                    }
                }
            }
		}

		protected override void Rollback()
		{
			base.Rollback();

			if (RollbackLevel >= ROLLBACK_LEVEL_FILE_OPERATION_BEGUN)
			{
				_DestinationFilePath.ValidateFileExist();
				File.Delete(_DestinationFilePath.FilePath);
			}	

			if( RollbackLevel >= ROLLBACK_LEVEL_TEMPORARY_FILE_CREATED )
				RestoreTemporaryFileIfExist();
		}

		#endregion
	}
}