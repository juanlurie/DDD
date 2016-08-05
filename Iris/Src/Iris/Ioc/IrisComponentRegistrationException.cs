using System;
using System.Runtime.Serialization;

namespace Iris.Ioc
{
    [Serializable]
    public class IrisComponentRegistrationException : Exception
    {
        public IrisComponentRegistrationException()
        {
        }

        public IrisComponentRegistrationException(string message) : base(message)
        {
        }

        public IrisComponentRegistrationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected IrisComponentRegistrationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}