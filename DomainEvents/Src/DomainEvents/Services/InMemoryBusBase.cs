using System;
using System.Collections;
using System.Reflection;
using DomainEvents.Contracts;

namespace DomainEvents.Services
{
    public abstract class InMemoryBusBase : IInMemoryBus
    {
        public void Publish(object @event)
        {
            IEnumerable matchingBlingHandlers = GetMatchingEventHandlers(@event);
            foreach (object handler in matchingBlingHandlers)
            {
                MethodInfo handlerMethod = handler.GetType().GetMethod("Handle", new[] { @event.GetType() });

                handlerMethod.Invoke(handler, new[] { @event });
            }
        }

        IEnumerable GetMatchingEventHandlers(object @event)
        {
            Type handlerType = typeof(IHandle<>);
            Type genericHandlerType = handlerType.MakeGenericType(@event.GetType());

            return ResolveAll(genericHandlerType);
        }

        protected abstract IEnumerable ResolveAll(Type type);
    }
}