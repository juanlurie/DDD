using System;
using System.Collections.Generic;
using System.Linq;
using Iris.Attributes;
using Iris.Extensions;
using Iris.Failover;
using Iris.Ioc;
using Iris.Messaging.Configuration.MessageHandlerCache;
using Iris.Reflection;

namespace Iris.Messaging.Configuration
{
    public class Configure : IConfigureEndpoint
    {
        private static readonly Configure Instance;
        private static IContainerBuilder containerBuilder;        

        static Configure()
        {
            Instance = new Configure();
        }    

        private Configure()
        {
            CriticalError.OnCriticalError += OnCriticalError;
        }

        public static Configure Initialize(string endpointName, IContainerBuilder builder)
        {
            Mandate.ParameterNotNullOrEmpty(endpointName, "endpointName");
            Mandate.ParameterNotNull(builder, "builder");

            containerBuilder = builder;
            containerBuilder.RegisterSingleton(containerBuilder);

            var busRegistrar = new MessageBusDependencyRegistrar();
            busRegistrar.Register(containerBuilder);           

            Settings.SetEndpointName(endpointName);
            Settings.RootContainer = containerBuilder.BuildContainer();            

            return Instance;
        }

        public IConfigureEndpoint DefineMessageAs(Func<Type, bool> isMessageRule)
        {
            Settings.IsMessageType = isMessageRule;
            return this;
        }

        public IConfigureEndpoint DefineCommandAs(Func<Type, bool> isCommandRule)
        {
            Settings.IsCommandType = isCommandRule;
            return this;
        }

        public IConfigureEndpoint DefineEventAs(Func<Type, bool> isEventRule)
        {
            Settings.IsEventType = isEventRule;
            return this;
        }

        public IConfigureEndpoint RegisterDependencies<T>() where T : IRegisterDependencies, new()
        {
            return RegisterDependencies(new T());
        }

        public IConfigureEndpoint RegisterDependencies(IRegisterDependencies registerationHolder)
        {
            registerationHolder.Register(containerBuilder);
            return this;
        }
        public IConfigureEndpoint DefiningMessagesAs(Func<Type, bool> definesMessageType)
        {
            Settings.IsMessageType = definesMessageType;
            return this;
        }

        public IConfigureEndpoint DefiningCommandsAs(Func<Type, bool> definesCommandType)
        {
            Settings.IsCommandType = definesCommandType;
            return this;
        }

        public IConfigureEndpoint DefiningEventsAs(Func<Type, bool> definesEventType)
        {
            Settings.IsEventType = definesEventType;
            return this;
        }        

        public IConfigureEndpoint UserNameResolver(Func<string> userNameResolver)
        {
            Settings.UserNameResolver = userNameResolver;
            return this;
        }

        public IConfigureEndpoint EndpointName(string name)
        {
            Settings.SetEndpointName(name);
            return this;
        }

        public IConfigureEndpoint EnableCommandValidators()
        {
            Settings.EnableCommandValidationClasses = true;
            return this;
        }

        internal void Start()
        {
            ComponentScanner.Scan(containerBuilder);

            MapMessageTypes();
            RunInitializers();
            StartServices();
        }

        private static void MapMessageTypes()
        {
            var mapper = Settings.RootContainer.GetInstance<ITypeMapper>();
            mapper.Initialize(HandlerCache.GetAllHandledMessageContracts());
        }

        private static void StartServices()
        {
            var startableObjects = Settings.RootContainer.GetAllInstances<IAmStartable>();

            foreach (var startableObject in startableObjects)
            {
                startableObject.Start();
            }
        }

        private static void RunInitializers()
        {
            var intializers = Settings.RootContainer.GetAllInstances<INeedToInitializeSomething>().ToArray();

            var orderedInitalizers = intializers
                .Where(something => something.HasAttribute<InitializationOrderAttribute>())
                .OrderBy(i => i.GetCustomAttributes<InitializationOrderAttribute>().First().Order);

            var unorderedInitalizers = intializers
                .Where(something => !something.HasAttribute<InitializationOrderAttribute>());

            RunIntializers(orderedInitalizers);
            RunIntializers(unorderedInitalizers);
        }

        private static void RunIntializers(IEnumerable<INeedToInitializeSomething> intializers)
        {
            foreach (var init in intializers)
            {
                init.Initialize();
            }
        }

        internal void Stop()
        {
            var startableObjects = Settings.RootContainer.GetAllInstances<IAmStartable>();

            foreach (var startableObject in startableObjects)
            {
                startableObject.Stop();
            }
        }

        private void OnCriticalError(CriticalErrorEventArgs e)
        {
            Stop();
        }
    }
}
