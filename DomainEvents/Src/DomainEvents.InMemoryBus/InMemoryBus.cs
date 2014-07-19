using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using DomainEvents.Contracts;

namespace DomainEvents.Dispatcher
{
    public class InMemoryBus : IInMemoryBus
    {
        private readonly IComponentContext container;

        public InMemoryBus(IComponentContext container)
        {
            this.container = container;
        }

        public void Publish(IEvent message)
        {
            IEnumerable eventHandlers = GetMatchingHandlers(message);
            foreach (object handler in eventHandlers)
            {
                MethodInfo handlerMethod = handler.GetType().GetMethod("Handle", new[] { message.GetType() });

                handlerMethod.Invoke(handler, new object[] { message });
            }
        }

        public void Send(ICommand command)
        {
            IEnumerable eventHandlers = GetMatchingHandlers(command);

            var handlers = GetCommandHandlers(command, eventHandlers);

            if (handlers.Count > 1)
                throw new Exception(string.Format("Cannot have multiple command handlers. {0}", command.GetType().Name));

            var item = handlers.First();
            item.Invoke(command);
        }

        private IEnumerable GetMatchingHandlers(object message)
        {
            Type handlerType = typeof(IHandle<>);
            Type genericHandlerType = handlerType.MakeGenericType(message.GetType());

            return ResolveAll(genericHandlerType);
        }

        private List<Action<ICommand>> GetCommandHandlers(ICommand command, IEnumerable eventHandlers)
        {
            var handlers = new List<Action<ICommand>>();
            foreach (object handler in eventHandlers)
            {
                var methodInfo = handler.GetType().GetMethod("Handle", new[] { command.GetType() });

                var methodHandler = handler;
                Action<ICommand> action = delegate { methodInfo.Invoke(methodHandler, new object[] { command }); };
                handlers.Add(action);
            }

            return handlers;
        }

        private IEnumerable ResolveAll(Type type)
        {
            Type interfaceGenericType = typeof(IEnumerable<>);
            Type interfaceType = interfaceGenericType.MakeGenericType(type);

            var handlers = container.Resolve(interfaceType);

            return (IEnumerable)handlers;
        }
    }
}