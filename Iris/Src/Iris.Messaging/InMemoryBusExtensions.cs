﻿using System.Threading.Tasks;

namespace Iris.Messaging
{
    public static class InMemoryBusExtensions
    {
        public static Task ExecuteAsync(this IInMemoryBus localBus, object command)
        {
            return Task.Factory.StartNew(o => localBus.Execute(command), command);
        }
    }
}