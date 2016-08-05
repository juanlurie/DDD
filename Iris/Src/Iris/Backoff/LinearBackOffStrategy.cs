using System;

namespace Iris.Backoff
{
    public class LinearBackOffStrategy : IBackOffStrategy
    {
        public TimeSpan NextDelay(TimeSpan currentDelay, TimeSpan initialDelay)
        {
            return new TimeSpan(currentDelay.Ticks + initialDelay.Ticks);
        }
    }
}