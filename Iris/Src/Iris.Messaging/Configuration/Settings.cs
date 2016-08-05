using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Iris.Ioc;

namespace Iris.Messaging.Configuration
{
    /// <summary>
    /// Used to store all infrastructure configuration settings
    /// </summary>
    public static class Settings
    {
        private const string EndpointNameSpace = ".Endpoint";
        private static int numberOfWorkers = 1;
        private static readonly Dictionary<string, string> settings = new Dictionary<string, string>();
        
        private static IContainer rootContainer;

        public static int SecondLevelRetryAttempts { get; internal set; }
        public static Func<Type, bool> IsMessageType { get; internal set; }
        public static Func<Type, bool> IsCommandType { get; internal set; }
        public static Func<Type, bool> IsEventType { get; internal set; }
        
        public static bool IsClientEndpoint { get; internal set; }
        public static bool SubsribeToDomainEvents { get; internal set; }
        public static bool DisablePerformanceMonitoring { get; internal set; }
        public static bool DisableHeartbeatService { get; internal set; }
        public static bool DisableMessageAudit { get; set; }
        public static TimeSpan CircuitBreakerReset { get; internal set; }
        public static int CircuitBreakerThreshold { get; internal set; }
        public static bool EnableCommandValidationClasses { get; internal set; }
        public static Func<string> UserNameResolver { get; internal set; }

        static Settings()
        {
            SecondLevelRetryAttempts = 0;
            CircuitBreakerReset = TimeSpan.FromSeconds(30);
            CircuitBreakerThreshold = 10;

            IsMessageType = type => false;
            IsCommandType = type => false;
            IsEventType = type => false;
        }

        public static IContainer RootContainer
        {
            get
            {
                if (rootContainer == null)
                    throw new InvalidOperationException("IoC container has not been built.");

                return rootContainer;
            }

            internal set { rootContainer = value; }
        }

        internal static void SetEndpointName(string endpointName)
        {
            Address.InitializeLocalAddress(ConfigureServiceName(endpointName));         
        }

        private static string ConfigureServiceName(string endpointName)
        {
            if (endpointName.EndsWith(EndpointNameSpace, true, CultureInfo.InvariantCulture))
            {
                return endpointName.Substring(0, endpointName.Length - EndpointNameSpace.Length);
            }

            return endpointName;
        }

        public static string GetSetting(string settingKey)
        {
            if (ConfigurationManager.AppSettings[settingKey] != null)
            {
                return ConfigurationManager.AppSettings[settingKey];
            }
            
            if (settings.ContainsKey(settingKey))
            {
                return settings[settingKey];
            }

            throw new ConfigurationSettingNotFoundException(settingKey);
        }

        public static void AddSetting(string key, string value)
        {
            settings[key] = value;
        }

        [Obsolete("Metod is deprecated, use the non-generic method GetSetting(string settingKey)", true)]
        public static T GetSetting<T>(string settingKey)
        {
            throw new NotImplementedException();
        }


        [Obsolete("Method is deprecated, use the method  AddSetting(string key, string value)", true)]
        public static void AddSetting(string settingKey, object value)
        {
            throw new NotImplementedException();
        }        
    }
}