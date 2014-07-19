using System;
using System.Reflection;
using Autofac;
using DomainEvents.Contracts;
using DomainEvents.Dispatcher;
using DomainEvents.Events;

namespace DomainEvents
{
    class Program
    {
        private static IContainer container;

        static void Main()
        {
            Initialise();

            var bus = container.Resolve<IInMemoryBus>();
            var command = new ChangeName("asd", "asd");
            bus.Send(command);

            Console.Read();
        }

        private static void Initialise()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IHandle<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<InMemoryBus>().As<IInMemoryBus>();

            container = builder.Build();
        }
    }
}
