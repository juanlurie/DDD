using System;

namespace Iris.Ioc
{
    public interface IContainerBuilder : IDisposable
    {
        IContainer BuildContainer();
        void RegisterType<T>(DependencyLifecycle dependencyLifecycle);
        void RegisterType<T>();
        void RegisterType(Type type, DependencyLifecycle dependencyLifecycle);
        void RegisterType(Type type);
        void RegisterSingleton(object instance);
        void RegisterModule(IRegisterDependencies module);
    }
}
