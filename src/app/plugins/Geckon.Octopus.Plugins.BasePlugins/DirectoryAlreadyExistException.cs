using System;
using System.Runtime.Serialization;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class DirectoryAlreadyExistException : Exception
	{
		public DirectoryAlreadyExistException()
		{
		}

		public DirectoryAlreadyExistException(string directoryPath)
			: base("Directory \"" + directoryPath + "\" already exists")
		{
		}

		public DirectoryAlreadyExistException(string directoryPath, Exception innerException)
			: base("Directory \"" + directoryPath + "\" already exists", innerException)
		{
		}

		public DirectoryAlreadyExistException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}