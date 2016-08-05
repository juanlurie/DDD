﻿using System;
using Hermes.EntityFramework;
using Hermes.Ioc;
using Hermes.Logging;
using Hermes.Messaging;
using Hermes.Messaging.EndPoints;
using Hermes.Messaging.Transports.SqlTransport;
using Hermes.ObjectBuilder.Autofac;
using Hermes.Serialization.Json;
using ProcessManagement.Contracts;
using ProcessManagement.Contracts.Commands;
using ProcessManagement.Persistence;

namespace ProcessManagement
{
    public class Endpoint : WorkerEndpoint<AutofacAdapter>
    {
        protected override void ConfigureEndpoint(IConfigureWorker configuration)
        {
            ConsoleWindowLogger.MinimumLogLevel = LogLevel.Info;

            configuration
                .FlushQueueOnStartup(true)
                .SecondLevelRetryPolicy(20, TimeSpan.FromMinutes(1))
                .UseJsonSerialization()
                .DisableHeartbeatService()
                .DisableMessageAudit()
                .DisablePerformanceMonitoring()
                .DefineCommandAs(IsCommand)
                .DefineEventAs(IsEvent)
                .DefineMessageAs(IsMessage)
                .RegisterDependencies<DependencyRegistration>()
                .UseSqlTransport("SqlTransport")
                .NumberOfWorkers(1)
                .RegisterMessageRoute<DebitCustomerCreditCard>(Address.Parse("ProcessManagement"))
                .RegisterMessageRoute<TimeoutReservation>(Address.Parse("ProcessManagement"))
                .ConfigureEntityFramework<ProcessManagerContext>("ProcessManagerContext");
        }

        private static bool IsCommand(Type type)
        {
            if (type == null || type.Namespace == null)
                return false;

            return typeof(ICommand).IsAssignableFrom(type);
        }

        private static bool IsEvent(Type type)
        {
            if (type == null || type.Namespace == null)
                return false;

            return typeof(IEvent).IsAssignableFrom(type);
        }

        private static bool IsMessage(Type type)
        {
            if (type == null || type.Namespace == null)
                return false;

            return typeof(IMessage).IsAssignableFrom(type);
        }
    }

    public class DependencyRegistration : IRegisterDependencies
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Blah>(DependencyLifecycle.SingleInstance);
        }
    }

    public class Blah : ScheduledWorkerService
    {
        public Blah()
        {
            RunImmediatelyOnStartup = true;
            this.WorkerThreads = 1;
        }

        protected override void DoWork()
        {
            var bus = ServiceLocator.Current.GetInstance<IInMemoryBus>();
            bus.Execute(new DummyCommand());
        }
    }

    public class DummyCommand : ICommand
    {
        
    }

    public class DummyCommandHandler : IHandleMessage<DummyCommand>
    {
        public void Handle(DummyCommand m)
        {
            Console.WriteLine("Hello");
        }
    }
}
