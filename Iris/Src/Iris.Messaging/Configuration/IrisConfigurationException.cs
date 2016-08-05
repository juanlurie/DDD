using System;
using System.Runtime.Serialization;

namespace Iris.Messaging.Configuration
{
    [Serializable]
    public class IrisConfigurationException : Exception
    {
        public IrisConfigurationException()
        {
        }

        public IrisConfigurationException(string message) : base(message)
        {
        }

        public IrisConfigurationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected IrisConfigurationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}