using System;
using System.Runtime.Serialization;

namespace Geckon.Octopus.Agent.Core.Exceptions
{
    public class NoExecutionSlotsAvailableException : Exception
    {
        public NoExecutionSlotsAvailableException()
            : base()
        {

        }

        public NoExecutionSlotsAvailableException(string message)
            : base(message)
        {

        }

        public NoExecutionSlotsAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public NoExecutionSlotsAvailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
