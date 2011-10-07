using System;
using Geckon.Octopus.Plugin.Interface;

namespace Geckon.Octopus.Core.Exceptions
{
	public class InvalidStatusToSetToPendingException : Exception
	{
		public InvalidStatusToSetToPendingException(PluginStatus status) : base("Current status (" + status + ") does not allow a change to a pending status")
		{
			
		}
	}
}
