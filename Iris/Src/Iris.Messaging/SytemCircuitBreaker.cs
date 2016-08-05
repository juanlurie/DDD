using System;
using Iris.Failover;
using Iris.Logging;
using Iris.Messaging.Configuration;

namespace Iris.Messaging
{
    public static class SytemCircuitBreaker
    {
        private static readonly CircuitBreaker CircuitBreaker;
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(SytemCircuitBreaker));

        static SytemCircuitBreaker()
        {
            CircuitBreaker = new CircuitBreaker(Settings.CircuitBreakerThreshold, Settings.CircuitBreakerReset);
        }

        public static void Trigger(Exception ex)
        {
            Logger.Error(ex.GetFullExceptionMessage());

            CircuitBreaker.Execute(() => CriticalError.Raise("Sytem Circuit Breaker Triggered", ex));
        }        

        public static void Trigger(Exception ex, string message)
        {
            Logger.Error(String.Format("{0}:\n{1}", message, ex.GetFullExceptionMessage()));
            CircuitBreaker.Execute(() => CriticalError.Raise(message, ex));
        }        
    }
}