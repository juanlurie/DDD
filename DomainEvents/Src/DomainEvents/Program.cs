using System;
using System.Reflection;
using Autofac;
using DomainEvents.Contracts;
using DomainEvents.Services;

namespace DomainEvents
{
    class Program
    {
        private static IContainer container;

        static void Main()
        {
            Initialise();

            var appService = container.Resolve<ApplicationService>();
            appService.UpdateName();

            Console.Read();
        }

        private static void Initialise()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IHandle<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationService>();
            builder.RegisterType<InMemoryBus>().As<IInMemoryBus>();

            container = builder.Build();
        }
    }
}
