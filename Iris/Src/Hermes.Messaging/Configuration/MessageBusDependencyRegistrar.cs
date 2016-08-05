using Hermes.Ioc;
using Hermes.Messaging.Bus;
using Hermes.Messaging.Callbacks;
using Hermes.Messaging.Pipeline;
using Hermes.Messaging.Pipeline.Modules;
using Hermes.Messaging.Transports;
using Hermes.Pipes;
using Hermes.Reflection;

namespace Hermes.Messaging.Configuration
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
