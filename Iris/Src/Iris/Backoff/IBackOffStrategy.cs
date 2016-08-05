using System;

namespace Iris.Backoff
{
    public interface IBackOffStrategy
    {
        TimeSpan NextDelay(TimeSpan currentDelay, TimeSpan initialDelay);
    }
}