using System;
using System.Runtime.Serialization;

namespace Geckon.Octopus.Plugins.BasePlugins
{
	public class PropertySetAfterOperationStartedException : Exception
	{
		public PropertySetAfterOperationStartedException()
		{
		}

		public PropertySetAfterOperationStartedException(string propertyName)
			:  base("Property " + propertyName + " attempted set after operation started")
		{
		}

		public PropertySetAfterOperationStartedException(string propertyName, Exception innerException)
			: base("Property " + propertyName + " attempted set after operation started", innerException)
		{
		}

		public PropertySetAfterOperationStartedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}