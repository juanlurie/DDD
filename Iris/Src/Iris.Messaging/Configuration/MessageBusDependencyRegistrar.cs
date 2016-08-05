using Iris.Ioc;
using Iris.Messaging.Bus;
using Iris.Messaging.Pipeline.Modules;
using Iris.Messaging.Transports;
using Iris.Reflection;

namespace Iris.Messaging.Configuration
{
    internal class MessageBusDependencyRegistrar : IRegisterDependencies
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Transport>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<LocalBus>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<Dispatcher>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<TypeMapper>(DependencyLifecycle.SingleInstance);

            containerBuilder.RegisterType<DispatchMessagesModule>(DependencyLifecycle.SingleInstance);
            
            containerBuilder.RegisterType<UnitOfWorkModule>(DependencyLifecycle.InstancePerUnitOfWork);
        }
    }
}
