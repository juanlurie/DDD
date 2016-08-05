using System;
using Microsoft.Practices.ServiceLocation;

namespace Iris.Ioc
{
    public interface IContainer : IServiceLocator, IDisposable
    {
        IContainer BeginLifetimeScope();
    }
}