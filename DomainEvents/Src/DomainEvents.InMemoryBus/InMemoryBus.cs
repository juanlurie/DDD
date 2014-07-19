using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
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

        public void Publish<TEvent>(TEvent message) where TEvent : IEvent
        {
            IEnumerable eventHandlers = GetMatchingHandlers(message);
            foreach (object handler in eventHandlers)
            {
                MethodInfo handlerMethod = handler.GetType().GetMethod("Handle", new[] { message.GetType() });

                var methodHandler = handler;
                Action<TEvent> action = delegate { handlerMethod.Invoke(methodHandler, new object[] { message }); };
                ThreadPool.QueueUserWorkItem(x => action(message));
            }
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            IEnumerable eventHandlers = GetMatchingHandlers(command);

            var handlers = GetCommandHandlers(command, eventHandlers);

            if (handlers.Count > 1)
                throw new Exception(string.Format("Cannot have multiple command handlers. {0}", command.GetType().Name));

            var item = handlers.First();

            RunAsync(command, item);
        }

        private void RunAsync<TCommand>(TCommand command, Action<TCommand> action) where TCommand : ICommand
        {
            action.BeginInvoke(command, Callback<TCommand>, action);
        }

        private void Callback<TCommand>(IAsyncResult asyncResult) where TCommand : ICommand
        {
            var asyncAction = (Action<TCommand>)asyncResult.AsyncState;
            asyncAction.EndInvoke(asyncResult);
        }

        private IEnumerable GetMatchingHandlers(object message)
        {
            Type handlerType = typeof(IHandle<>);
            Type genericHandlerType = handlerType.MakeGenericType(message.GetType());

            return ResolveAll(genericHandlerType);
        }

        private List<Action<TCommand>> GetCommandHandlers<TCommand>(TCommand command, IEnumerable eventHandlers) where TCommand : ICommand
        {
            var handlers = new List<Action<TCommand>>();
            foreach (object handler in eventHandlers)
            {
                var methodInfo = handler.GetType().GetMethod("Handle", new[] { command.GetType() });

                var methodHandler = handler;
                Action<TCommand> action = delegate { methodInfo.Invoke(methodHandler, new object[] { command }); };
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