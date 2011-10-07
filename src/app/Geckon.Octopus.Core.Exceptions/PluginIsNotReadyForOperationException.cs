using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geckon.Octopus.Core.Exceptions
{
	public class PluginIsNotReadyForOperationException : Exception
	{
		public PluginIsNotReadyForOperationException()
			: base("Plugin is not ready to begin operation")
		{
			
		}
	}
}
