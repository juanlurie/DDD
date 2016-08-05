﻿using System;
using System.Collections.Generic;
using System.Threading;
using Iris.Ioc;
using Iris.Logging;
using Iris.Messaging.Configuration;
using Iris.Messaging.Pipeline;
using Iris.Messaging.Transports;
using ServiceLocator = Iris.Ioc.ServiceLocator;

namespace Iris.Messaging.Bus
{
    public class LocalBus : IInMemoryBus
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof (LocalBus));

        private readonly IContainer container;
        private readonly ITransportMessages messageTransport;
        private readonly IDispatchMessagesToHandlers dispatcher;

        private readonly List<int> executingThreads = new List<int>();

        public LocalBus(ITransportMessages messageTransport, IContainer container, IDispatchMessagesToHandlers dispatcher)
        {
            this.messageTransport = messageTransport;
            this.dispatcher = dispatcher;
            this.container = container;
        }

        public void Execute(object command)
        {
            Mandate.ParameterNotNull(command, "command");
            MessageRuleValidation.ValidateCommand(command);

            if (messageTransport.CurrentMessage.MessageId != Guid.Empty)
                throw new InvalidOperationException("A command may not be executed while another command is being processed.");

            int threadId = Thread.CurrentThread.ManagedThreadId;

            try
            {
                lock (executingThreads)
                {
                    if (executingThreads.Contains(threadId))
                        throw new InvalidOperationException("A command may not be executed while another command is being processed.");

                    executingThreads.Add(threadId);
                }

                ProcessCommand(command);
            }
            catch (CommandValidationException ex)
            {
                string description = ex.GetDescription();
                Logger.Warn(description);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.GetFullExceptionMessage());
                throw;
            }
            finally
            {
                lock (executingThreads)
                {
                    executingThreads.Remove(threadId);
                }
            }
        }

        protected virtual void ProcessCommand(object message)
        {
            Logger.Info("User [{0}] Executing : {1}", CurrentUser.GetCurrentUserName(), message);

            if (ServiceLocator.Current.IsDisposed())
            {
                ProcessCommandWithNewLifetimeScope(message);
            }
            else
            {
                ProcessCommandWithExistingLifetimeScope(message);
            }
        }

        private void ProcessCommandWithExistingLifetimeScope(object message)
        {
            Logger.Debug("Process command with existing lifetimeScope");
            var incomingContext = new IncomingMessageContext(message, ServiceLocator.Current);
            messageTransport.ProcessMessage(incomingContext);
        }

        private void ProcessCommandWithNewLifetimeScope(object message)
        {
            try
            {
                Logger.Debug("Process command with new lifetimeScope");

                using (IContainer childContainer = container.BeginLifetimeScope())
                {
                    ServiceLocator.Current.SetCurrentLifetimeScope(childContainer);
                    var incomingContext = new IncomingMessageContext(message, childContainer);
                    messageTransport.ProcessMessage(incomingContext);
                }
            }
            finally
            {
                ServiceLocator.Current.SetCurrentLifetimeScope(null);
            }
        }

        public void Raise(object @event)
        {
            if(!ServiceLocator.Current.HasServiceProvider())
                throw new InvalidOperationException("A local event may only be raised within the context of an executing local command or received message.");

            MessageRuleValidation.ValidateEvent(@event);
            Logger.Info("Raising : {0}", @event);
            dispatcher.DispatchToHandlers(@event, ServiceLocator.Current);
        }
    }
}