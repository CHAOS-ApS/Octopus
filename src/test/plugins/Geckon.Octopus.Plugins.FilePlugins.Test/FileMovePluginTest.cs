using System;
using System.IO;
using Geckon.Events;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.BasePlugins;
using NUnit.Framework;

namespace Geckon.Octopus.Plugins.FilePlugins.Test
{
	[TestFixture]
	public class FileMovePluginTest
	{

		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
		}

		#endregion

		[Test, ExpectedException(typeof(FileNotFoundException))]
		public void ShouldFailWithFileNotFoundExceptionWhenSourceFileDoesNotExistsBeforeBeginExecute()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;

			var plugin = new FileMovePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			while (sourceFilePath == null || File.Exists(sourceFilePath))
				sourceFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

			while (destinationFilePath == null || File.Exists(destinationFilePath))
				destinationFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

			plugin.SourceFilePath = sourceFilePath;
			plugin.DestinationFilePath = destinationFilePath;

			plugin.BeginExecute();
		}

		[Test, ExpectedException(typeof(DirectoryNotFoundException))]
		public void ShouldFailWithDirectoryNotFoundExceptionWhenDestinationDirectoryDoesNotExistsBeforeBeginExecute()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			FileMovePlugin plugin = null;

			try
			{
				sourceFilePath = Path.GetTempFileName();
				destinationFilePath = Path.Combine(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()),
												   Path.GetRandomFileName());
				plugin = new FileMovePlugin();
				plugin.ExecuteFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;

				plugin.BeginExecute();
			}
			finally
			{
				if (File.Exists(sourceFilePath))
					File.Delete(sourceFilePath);
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
			}
		}

		[Test, ExpectedException(typeof(FileAlreadyExistException))]
		public void ShouldFailWithFileAlreadyExistExceptionWhenDestinationFileAlreadyExistsBeforeBeginExecute()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			FileMovePlugin plugin = null;

			try
			{
				sourceFilePath = Path.GetTempFileName();
				destinationFilePath = Path.GetTempFileName();
				plugin = new FileMovePlugin();
				plugin.ExecuteFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;

				plugin.BeginExecute();
			}
			finally
			{
				if (File.Exists(sourceFilePath))
					File.Delete(sourceFilePath);
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
			}
		}

		[Test]
		public void ShouldCopyFileOnBeginExecute()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			FileMovePlugin plugin = null;

			try
			{
				var fileContent = Guid.NewGuid().ToString();

				sourceFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				File.WriteAllText(sourceFilePath, fileContent);

				destinationFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				plugin = new FileMovePlugin();
				plugin.ExecuteFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;

				plugin.BeginExecute();

				Assert.That(File.Exists(destinationFilePath));
				Assert.AreEqual(fileContent, File.ReadAllText(destinationFilePath));
			}
			finally
			{
				if (File.Exists(sourceFilePath))
					File.Delete(sourceFilePath);
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
			}
		}

		[Test, ExpectedException(typeof(FileNotFoundException))]
		public void ShouldFailWithFileNotFoundExceptionWhenOverwritedTemporaryFilePathDoesNotExistOnBeginComit()
		{
			var overwritedTemporaryFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var plugin = new FileMovePlugin();

			plugin.ShouldOwerwriteExistingFile = true;
			plugin.TemporaryFilePath = overwritedTemporaryFilePath;

			plugin.CommitFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			plugin.BeginCommit();
		}

		[Test]
		public void ShouldDeleteOverwrittenFileOnBeginComit()
		{
			String overwrittenFilePath = null;
			FileMovePlugin plugin = null;

			try
			{
				overwrittenFilePath = Path.GetTempFileName();

				plugin = new FileMovePlugin();
				plugin.CommitFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.ShouldOwerwriteExistingFile = true;
				plugin.TemporaryFilePath = overwrittenFilePath;

				plugin.BeginCommit();

				Assert.IsFalse(File.Exists(overwrittenFilePath));
			}
			finally
			{
				if (File.Exists(overwrittenFilePath))
					File.Delete(overwrittenFilePath);
			}
		}

		[Test, ExpectedException(typeof(FileAlreadyExistException))]
		public void ShouldFailWithFileAlreadyExistExceptionWhenSourceFilePathAlreadyExistOnBeginRollback()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			FileMovePlugin plugin = null;

			try
			{
				sourceFilePath = Path.GetTempFileName();
				destinationFilePath = Path.GetTempFileName();

				plugin = new FileMovePlugin();
				plugin.RollbackLevel = RollbackLevels.FINAL_LEVEL;
				plugin.RollbackFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;

				plugin.BeginRollback();
			}
			finally
			{
				if (File.Exists(sourceFilePath))
					File.Delete(sourceFilePath);
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
			}
		}

		[Test, ExpectedException(typeof(FileNotFoundException))]
		public void ShouldFailWithFileNotFoundExceptionWhenOverwritedTemporaryFilePathDoesNotExistOnBeginRollback()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			String overwrittenFilePath = null;
			FileMovePlugin plugin = null;

			try
			{
				sourceFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				overwrittenFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				destinationFilePath = Path.GetTempFileName();

				plugin = new FileMovePlugin();
				plugin.RollbackLevel = RollbackLevels.FINAL_LEVEL;
				plugin.RollbackFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;
				plugin.ShouldOwerwriteExistingFile = true;
				plugin.TemporaryFilePath = overwrittenFilePath;

				plugin.BeginRollback();
			}
			finally
			{
				if (File.Exists(sourceFilePath))
					File.Delete(sourceFilePath);
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
			}
		}

		[Test]
		public void ShouldRestoreOverwrittenFileOnBeginRollback()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			String overwrittenFilePath = null;
			FileMovePlugin plugin = null;

			try
			{
				var fileContent = Guid.NewGuid().ToString();

				overwrittenFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				File.WriteAllText(overwrittenFilePath, fileContent);

				sourceFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				destinationFilePath = Path.GetTempFileName();
				plugin = new FileMovePlugin();
				plugin.RollbackLevel = RollbackLevels.FINAL_LEVEL;
				plugin.RollbackFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;
				plugin.ShouldOwerwriteExistingFile = true;
				plugin.TemporaryFilePath = overwrittenFilePath;

				plugin.BeginRollback();

				Assert.That(File.Exists(destinationFilePath));
				Assert.AreEqual(fileContent, File.ReadAllText(destinationFilePath));
			}
			finally
			{
				if (File.Exists(sourceFilePath))
					File.Delete(sourceFilePath);
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
				if (File.Exists(overwrittenFilePath))
					File.Delete(overwrittenFilePath);
			}
		}
	}
}