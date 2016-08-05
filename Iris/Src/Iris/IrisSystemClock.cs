using System;
using System.Threading;
using Iris.Logging;

namespace Iris
{
    public static class IrisSystemClock 
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof (IrisSystemClock));

        private static readonly ThreadLocal<Func<DateTimeOffset>> CurrentResolver = new ThreadLocal<Func<DateTimeOffset>>();

        public static DateTimeOffset UtcNow { get { return ResolveCurrentUtcTime(); } }

        public static void SetUtcResolver(Func<DateTimeOffset> resolveUtcNow)
        {
            Mandate.ParameterNotNull(resolveUtcNow, "resolveUtcNow");

            CurrentResolver.Value = resolveUtcNow;
        }

        private static DateTimeOffset ResolveCurrentUtcTime()
        {
            try
            {
                var resolver = CurrentResolver.Value ?? (() => DateTimeOffset.UtcNow);
                return resolver();
            }
            catch(Exception ex)
            {
                Logger.Error("Error resolving current UTC time : {0}", ex.GetFullExceptionMessage());
                return new DateTimeOffset();
            }
        }
    }
}