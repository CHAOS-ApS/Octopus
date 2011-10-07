using System;
using System.Runtime.Serialization;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class FileAlreadyExistException : Exception
	{
		public FileAlreadyExistException()
		{
		}

		public FileAlreadyExistException(string filePath)
			: base("File \"" + filePath + "\" already exists")
		{
		}

		public FileAlreadyExistException(string filePath, Exception innerException)
			: base("File \"" + filePath + "\" already exists", innerException)
		{
		}

		public FileAlreadyExistException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}