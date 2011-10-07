using System;
using System.Runtime.Serialization;

namespace Geckon.Octopus.Core.Exceptions
{
    public class NoOpenExecutionSlotsException : Exception
    {
        public NoOpenExecutionSlotsException()
            : base()
        {

        }

        public NoOpenExecutionSlotsException(string message)
            : base(message)
        {

        }

        public NoOpenExecutionSlotsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public NoOpenExecutionSlotsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}