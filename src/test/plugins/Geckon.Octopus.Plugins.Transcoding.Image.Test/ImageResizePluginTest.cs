using System;
using System.Drawing;
using System.IO;
using Geckon.Events;
using Geckon.Graphics;
using Geckon.Octopus.Plugin.Core;
using Geckon.Octopus.Plugin.Interface;
using Geckon.Octopus.Plugins.BasePlugins;
using Geckon.Octopus.TestUtilities;
using Geckon.Serialization.Xml;
using NUnit.Framework;

namespace Geckon.Octopus.Plugins.Transcoding.Image.Test
{
	[TestFixture]
	public class ImageResizePluginTest
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

		[Test, ExpectedException(typeof (PropertyNotSetBeforeOperationException))]
		public void ShouldFailWithPropertyNotSetBeforeOperationExceptionWhenNoPropertiesSetBeforeBeginExecute()
		{
			var plugin = new ImageResizePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			plugin.BeginExecute();
		}

		[Test, ExpectedException(typeof (PropertyNotSetBeforeOperationException))]
		public void ShouldFailWithPropertyNotSetBeforeOperationExceptionWhenSourceFilePathNotSetBeforeBeginExecute()
		{
			var plugin = new ImageResizePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			plugin.DestinationFilePath = Path.GetRandomFileName();

			plugin.BeginExecute();
		}

		[Test, ExpectedException(typeof (PropertyNotSetBeforeOperationException))]
		public void ShouldFailWithPropertyNotSetBeforeOperationExceptionWhenDestinationFilePathNotSetBeforeBeginExecute()
		{
			var plugin = new ImageResizePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			plugin.SourceFilePath = Path.GetRandomFileName();

			plugin.BeginExecute();
		}

		[Test, ExpectedException(typeof (FileNotFoundException))]
		public void ShouldFailWithFileNotFoundExceptionWhenSourceFileDoesNotExistsBeforeBeginExecute()
		{
			String sourceFilePath;
			String destinationFilePath;

			var plugin = new ImageResizePlugin();
			plugin.ExecuteFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			sourceFilePath = FileOperations.GetNonExistentFilePath();
			destinationFilePath = FileOperations.GetNonExistentFilePath();

			plugin.SourceFilePath = sourceFilePath;
			plugin.DestinationFilePath = destinationFilePath;
			plugin.Amount = 1;
			plugin.Type = ResizeType.percent;
			plugin.Quality = 100;

			plugin.BeginExecute();
		}

		[Test, ExpectedException(typeof (DirectoryNotFoundException))]
		public void ShouldFailWithDirectoryNotFoundExceptionWhenDestinationDirectoryDoesNotExistsBeforeBeginExecute()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			ImageResizePlugin plugin;

			try
			{
				sourceFilePath = Path.GetTempFileName();
				destinationFilePath = Path.Combine(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()),
				                                   Path.GetRandomFileName());
				plugin = new ImageResizePlugin();
				plugin.ExecuteFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;
				plugin.Amount = 1;
				plugin.Type = ResizeType.percent;
				plugin.Quality = 100;

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

		[Test, ExpectedException(typeof (FileAlreadyExistException))]
		public void ShouldFailWithFileAlreadyExistExceptionWhenDestinationFileAlreadyExistsBeforeBeginExecute()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			ImageResizePlugin plugin;

			try
			{
				sourceFilePath = Path.GetTempFileName();
				destinationFilePath = Path.GetTempFileName();
				plugin = new ImageResizePlugin();
				plugin.ExecuteFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;
				plugin.Amount = 1;
				plugin.Type = ResizeType.percent;
				plugin.Quality = 100;

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

		[Test, ExpectedException(typeof (FileNotFoundException))]
		public void ShouldFailWithFileNotFoundExceptionWhenOverwritedTemporaryFilePathDoesNotExistOnBeginComit()
		{
			var overwritedTemporaryFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var plugin = new ImageResizePlugin();

			plugin.ShouldOwerwriteExistingFile = true;
			plugin.TemporaryFilePath = overwritedTemporaryFilePath;

			plugin.CommitFailed +=
				delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

			plugin.BeginCommit();
		}

		[Test]
		public void ShouldDeleteOverwrittenFileOnBeginComit()
		{
			String temporaryFilePath = null;
			ImageResizePlugin plugin;

			try
			{
				temporaryFilePath = Path.GetTempFileName();

				plugin = new ImageResizePlugin();
				plugin.CommitFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.ShouldOwerwriteExistingFile = true;
				plugin.TemporaryFilePath = temporaryFilePath;

				plugin.BeginCommit();

				Assert.IsFalse(File.Exists(temporaryFilePath));
			}
			finally
			{
				if (File.Exists(temporaryFilePath))
					File.Delete(temporaryFilePath);
			}
		}

		[Test, ExpectedException(typeof (FileNotFoundException))]
		public void ShouldFailWithFileNotFoundExceptionWhenOverwritedTemporaryFilePathDoesNotExistOnBeginRollback()
		{
			String destinationFilePath = null;
			String temporaryFilePath;
			ImageResizePlugin plugin;

			try
			{
				temporaryFilePath = FileOperations.GetNonExistentFilePath();

				destinationFilePath = Path.GetTempFileName();
				plugin = new ImageResizePlugin();
				plugin.RollbackLevel = RollbackLevels.FINAL_LEVEL;
				plugin.RollbackFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.DestinationFilePath = destinationFilePath;
				plugin.ShouldOwerwriteExistingFile = true;
				plugin.TemporaryFilePath = temporaryFilePath;

				plugin.BeginRollback();
			}
			finally
			{
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
			}
		}

		[Test]
		public void ShouldRestoreOverwrittenFileOnBeginRollback()
		{
			String destinationFilePath = null;
			String temporaryFilePath = null;
			ImageResizePlugin plugin;

			try
			{
				var fileContent = Guid.NewGuid().ToString();

				temporaryFilePath = FileOperations.GetNonExistentFilePath();
				File.WriteAllText(temporaryFilePath, fileContent);

				destinationFilePath = Path.GetTempFileName();
				plugin = new ImageResizePlugin();
				plugin.RollbackLevel = RollbackLevels.FINAL_LEVEL;
				plugin.RollbackFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.DestinationFilePath = destinationFilePath;
				plugin.ShouldOwerwriteExistingFile = true;
				plugin.TemporaryFilePath = temporaryFilePath;

				plugin.BeginRollback();

				Assert.That(File.Exists(destinationFilePath));
				Assert.AreEqual(fileContent, File.ReadAllText(destinationFilePath));
			}
			finally
			{
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
				if (File.Exists(temporaryFilePath))
					File.Delete(temporaryFilePath);
			}
		}

		[Test]
		public void ShouldResizeImage()
		{
			String sourceFilePath = null;
			String destinationFilePath = null;
			ImageResizePlugin plugin;

			try
			{
				sourceFilePath = CreateImage(500, 500);
				destinationFilePath = FileOperations.GetNonExistentFilePath("jpg");

				plugin = new ImageResizePlugin();
				plugin.ExecuteFailed +=
					delegate(object sender, ObjectErrorEventArgs<IPlugin> e) { throw e.Exception; };

				plugin.SourceFilePath = sourceFilePath;
				plugin.DestinationFilePath = destinationFilePath;
			    plugin.Amount = 530;
				plugin.Type = ResizeType.width;
				plugin.Quality = 95;

                Console.WriteLine( XmlSerialize.ToXML( plugin ).OuterXml );

				plugin.BeginExecute();

				using (var image = new Bitmap(destinationFilePath))
				{
					Assert.AreEqual(1000, image.Width);
				}
			}
			finally
			{
				if (File.Exists(sourceFilePath))
					File.Delete(sourceFilePath);
				if (File.Exists(destinationFilePath))
					File.Delete(destinationFilePath);
			}
		}

		private static string CreateImage(int width, int height)
		{
			var image = new Bitmap(width, height);

			var path = FileOperations.GetNonExistentFilePath("jpg");

			image.Save(path);

			return path;
		}
	}
}