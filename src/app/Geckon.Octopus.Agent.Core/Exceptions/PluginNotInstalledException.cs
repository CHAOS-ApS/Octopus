using System;
using System.Runtime.Serialization;

namespace Geckon.Octopus.Agent.Core.Exceptions
{
    public class PluginNotInstalledException : Exception
    {
        public PluginNotInstalledException()
            : base()
        {

        }

        public PluginNotInstalledException(string message)
            : base(message)
        {

        }

        public PluginNotInstalledException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public PluginNotInstalledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}