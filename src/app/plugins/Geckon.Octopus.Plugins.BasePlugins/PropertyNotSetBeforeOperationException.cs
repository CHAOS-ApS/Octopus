using System;
using System.Runtime.Serialization;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PropertyNotSetBeforeOperationException : Exception
	{
		public PropertyNotSetBeforeOperationException()
		{
		}

		public PropertyNotSetBeforeOperationException(string propertyName)
			: base("Property " + propertyName + " not set before operation")
		{
		}

		public PropertyNotSetBeforeOperationException(string propertyName, Exception innerException)
			: base("Property " + propertyName + " not set before operation", innerException)
		{
		}

		public PropertyNotSetBeforeOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}