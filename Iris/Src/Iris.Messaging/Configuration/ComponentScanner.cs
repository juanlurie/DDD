using System;
using System.Collections.Generic;
using System.Linq;
using Iris.Equality;
using Iris.Ioc;
using Iris.Messaging.Configuration.MessageHandlerCache;
using Iris.Reflection;

namespace Iris.Messaging.Configuration
{
    internal static class ComponentScanner
    {
        public static void Scan(IContainerBuilder containerBuilder)
        {
                IEnumerable<Type> messageTypes = GetMessageTypes();
                ICollection<Type> messageHandlerTypes = GetMessageHandlerTypes();
                ICollection<Type> commandValidatorTypes = GetCommandValidatorTypes();
                ICollection<Type> intializerTypes = GetInitializerTypes();

                HandlerCache.InitializeCache(messageTypes, messageHandlerTypes);

                RegisterTypes(containerBuilder, messageHandlerTypes, DependencyLifecycle.InstancePerUnitOfWork);
                RegisterTypes(containerBuilder, commandValidatorTypes, DependencyLifecycle.InstancePerUnitOfWork);
                RegisterTypes(containerBuilder, intializerTypes, DependencyLifecycle.SingleInstance);
        }

        private static void RegisterTypes(IContainerBuilder containerBuilder, IEnumerable<Type> types, DependencyLifecycle dependencyLifecycle)
        {
            foreach (var type in types)
            {
                containerBuilder.RegisterType(type, dependencyLifecycle);
            }
        }

        private static IEnumerable<Type> GetMessageTypes()
        {
            return AssemblyScanner.Types
                          .Where(Settings.IsCommandType)
                          .Union(AssemblyScanner.Types.Where(Settings.IsEventType))
                          .Union(AssemblyScanner.Types.Where(Settings.IsMessageType))
                          .Distinct(new TypeEqualityComparer());
        }

        private static ICollection<Type> GetMessageHandlerTypes()
        {
            return
                AssemblyScanner.Types.Where(
                    t => !t.IsAbstract &&  
                        t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleMessage<>)))
                       .Distinct(new TypeEqualityComparer())
                       .ToArray();
        }

        private static ICollection<Type> GetCommandValidatorTypes()
        {
            return
                AssemblyScanner.Types.Where(
                    t => !t.IsAbstract &&
                        t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidateCommand<>)))
                       .Distinct(new TypeEqualityComparer())
                       .ToArray();
        }

        private static ICollection<Type> GetInitializerTypes()
        {
            return AssemblyScanner.Types
                .Where(t => typeof (INeedToInitializeSomething).IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();
        }
    }
}