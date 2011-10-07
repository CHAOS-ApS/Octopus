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
	public class FileDeletePluginTest
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

		[Test]
		public void ShouldBeAbleToGetFilePathAfterSet()
		{
			var plugin = new FileDeletePlugin();

			var tmpFilePath = Path.GetRandomFileName();

			plugin.FilePath = tmpFilePath;

			Assert.AreEqual(tmpFilePath, plugin.FilePath);
		}

		[Test, ExpectedException(typeof (PropertyNotSetBeforeOperationException))]
		public void ShouldFailWithPropertyNotSetBeforeOperationExceptionWhenFilePathNotSetBeforeBeginExecute()
		{
			var plugin = new FileDeletePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			plugin.BeginExecute();
		}

		[Test, ExpectedException(typeof (ArgumentNullException))]
		public void ShouldThrowArgumentExceptiondWhenFilePathSetToNull()
		{
			var plugin = new FileDeletePlugin();

			plugin.FilePath = null;
		}

		[Test, ExpectedException(typeof (ArgumentException))]
		public void ShouldThrowArgumentExceptionWhenFilePathSetToEmptyString()
		{
			var plugin = new FileDeletePlugin();

			plugin.FilePath = "";
		}

		[Test, ExpectedException(typeof (PropertySetAfterOperationStartedException))]
		public void ShouldThrowPropertySetAfterOperationStartedExceptionWhenSettingFilePathAfterBeginExecute()
		{
			String filePath = null;
			FileDeletePlugin plugin = null;

			try
			{
				filePath = Path.GetTempFileName();
				plugin = new FileDeletePlugin();

				plugin.FilePath = filePath;

				plugin.BeginExecute();

				plugin.FilePath = Path.GetRandomFileName();
			}
			finally
			{
				if (File.Exists(filePath))
					File.Delete(filePath);
				else if (File.Exists(plugin.TemporaryFilePath))
					File.Delete(plugin.TemporaryFilePath);
			}
		}

		[Test, ExpectedException(typeof (FileNotFoundException))]
		public void ShouldThrowFileNotFoundExceptionWhenFileDoesNotExistOnBeginExecute()
		{
			String filePath = null;
			FileDeletePlugin plugin = null;

			while (filePath == null || File.Exists(filePath))
				filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

			plugin = new FileDeletePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			plugin.FilePath = filePath;

			plugin.BeginExecute();
		}

		[Test]
		public void ShouldMoveFileToTemporaryLocationOnExecuteBegin()
		{
			String filePath = null;
			FileDeletePlugin plugin = null;

			try
			{
				filePath = Path.GetTempFileName();
				plugin = new FileDeletePlugin();
				plugin.ExecuteFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.FilePath = filePath;

				Assert.IsTrue(File.Exists(filePath));

				plugin.BeginExecute();

				Assert.IsFalse(File.Exists(filePath));
				Assert.IsTrue(File.Exists(plugin.TemporaryFilePath));
			}
			finally
			{
				if (File.Exists(filePath))
					File.Delete(filePath);
				else if (File.Exists(plugin.TemporaryFilePath))
					File.Delete(plugin.TemporaryFilePath);
			}
		}

		[Test]
		public void ShouldDeleteFileOnComitBegin()
		{
			String temporaryFilePath = null;
			FileDeletePlugin plugin = null;

			try
			{
				temporaryFilePath = Path.GetTempFileName();
				plugin = new FileDeletePlugin();
				plugin.CommitFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.FilePath = Path.GetRandomFileName();
				plugin.TemporaryFilePath = temporaryFilePath;

				Assert.IsTrue(File.Exists(temporaryFilePath));

				plugin.BeginCommit();

				Assert.IsFalse(File.Exists(temporaryFilePath));
			}
			finally
			{
				if (File.Exists(temporaryFilePath))
					File.Delete(temporaryFilePath);
				else if (File.Exists(plugin.FilePath))
					File.Delete(plugin.FilePath);
			}
		}

		[Test, ExpectedException(typeof (FileAlreadyExistException))]
		public void ShouldThrowFileAlreadyExistExceptionWhenFileExistOnBeginRollbackBegin()
		{
			String filePath = null;
			String temporaryFilePath = null;
			FileDeletePlugin plugin = null;

			try
			{
				filePath = Path.GetTempFileName();
				temporaryFilePath = Path.GetTempFileName();
				plugin = new FileDeletePlugin();
				plugin.RollbackLevel = RollbackLevels.FINAL_LEVEL;
				plugin.RollbackFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.FilePath = filePath;
				plugin.TemporaryFilePath = temporaryFilePath;

				plugin.BeginRollback();
			}
			finally
			{
				if (File.Exists(filePath))
					File.Delete(filePath);
				if (File.Exists(temporaryFilePath))
					File.Delete(temporaryFilePath);
			}
		}

		[Test]
		public void ShouldRestoreFileOnRollbackBegin()
		{
			String filePath = null;
			String temporaryFilePath = null;
			FileDeletePlugin plugin = null;

			try
			{
				filePath = Path.GetTempPath() + Path.GetRandomFileName();
				temporaryFilePath = Path.GetTempFileName();
				plugin = new FileDeletePlugin();
				plugin.RollbackLevel = RollbackLevels.FINAL_LEVEL;
				plugin.RollbackFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.FilePath = filePath;
				plugin.TemporaryFilePath = temporaryFilePath;

				Assert.IsFalse(File.Exists(filePath));

				plugin.BeginRollback();

				Assert.IsTrue(File.Exists(filePath));
			}
			finally
			{
				if (File.Exists(filePath))
					File.Delete(filePath);
				else if (File.Exists(temporaryFilePath))
					File.Delete(temporaryFilePath);
			}
		}
	}
}