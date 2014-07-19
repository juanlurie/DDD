using System;
using System.Collections;
using System.Collections.Generic;
using Autofac;

namespace DomainEvents.Services
{
    public class InMemoryBus : InMemoryBusBase
    {
        readonly IComponentContext container;

        public InMemoryBus(IComponentContext container)
        {
            this.container = container;
        }

        protected override IEnumerable ResolveAll(Type type)
        {
            Type interfaceGenericType = typeof(IEnumerable<>);
            Type interfaceType = interfaceGenericType.MakeGenericType(type);

            var handlers = container.Resolve(interfaceType);

            return (IEnumerable)handlers;
        }
    }
}