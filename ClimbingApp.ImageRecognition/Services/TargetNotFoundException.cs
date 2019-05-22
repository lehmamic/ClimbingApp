using System;
using System.Runtime.Serialization;

namespace ClimbingApp.ImageRecognition.Services
{
    public class TargetNotFoundException : Exception
    {
        public TargetNotFoundException()
            : base()
        {
        }

        protected TargetNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public TargetNotFoundException(string message)
            : base(message)
        {
        }

        public TargetNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
