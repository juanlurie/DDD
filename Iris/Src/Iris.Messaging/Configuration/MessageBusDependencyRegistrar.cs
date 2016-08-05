using Iris.Ioc;
using Iris.Messaging.Bus;
using Iris.Messaging.Pipeline;
using Iris.Messaging.Pipeline.Modules;
using Iris.Messaging.Transports;
using Iris.Pipes;
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

            containerBuilder.RegisterType<AuditModule>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<ExtractMessagesModule>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<MessageMutatorModule>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<DispatchMessagesModule>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<CallBackHandlerModule>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<MessageSerializationModule>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<HeaderBuilderModule>(DependencyLifecycle.SingleInstance);
            containerBuilder.RegisterType<EnqueuedMessageSenderModule>(DependencyLifecycle.SingleInstance);
            
            containerBuilder.RegisterType<UnitOfWorkModule>(DependencyLifecycle.InstancePerUnitOfWork);

            var outgoingPipeline = new ModulePipeFactory<OutgoingMessageContext>()
              .Add<MessageSerializationModule>()
              .Add<MessageMutatorModule>()
              .Add<HeaderBuilderModule>()
              .Add<CommandValidationModule>()
              .Add<SendMessageModule>();
            containerBuilder.RegisterSingleton(outgoingPipeline);
        }
    }
}
