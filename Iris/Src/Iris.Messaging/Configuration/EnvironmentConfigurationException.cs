using System;

namespace Iris.Messaging.Configuration
{
    public class EnvironmentConfigurationException : Exception
    {
        public EnvironmentConfigurationException(string message)
            :base(message)
        {
        }       
    }
}