using System.IO;

namespace Geckon.Octopus.TestUtilities
{
	public static class FileOperations
	{
		public static string GetNonExistentFilePath()
		{
			string path = null;

			while (path == null || File.Exists(path))
				path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

			return path;
		}

		public static string GetNonExistentFilePath(string extension)
		{
			string path = null;

			while (path == null || File.Exists(path))
				path = Path.ChangeExtension(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()), extension);

			return path;
		}
	}
}