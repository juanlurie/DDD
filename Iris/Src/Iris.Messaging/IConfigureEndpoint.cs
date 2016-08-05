using System;
using Iris.Ioc;

namespace Iris.Messaging
{
    public interface IConfigureEndpoint
    {
        IConfigureEndpoint RegisterDependencies(IRegisterDependencies registerationHolder);
        IConfigureEndpoint RegisterDependencies<T>() where T : IRegisterDependencies, new();
        IConfigureEndpoint DefineMessageAs(Func<Type, bool> isMessageRule);
        IConfigureEndpoint DefineCommandAs(Func<Type, bool> isCommandRule);
        IConfigureEndpoint DefineEventAs(Func<Type, bool> isEventRule);
        IConfigureEndpoint UserNameResolver(Func<string> resolveUserName);
        IConfigureEndpoint EndpointName(string name);

        /// <summary>
        /// Enables validation through classes which implement IValidateCommand. 
        /// </summary>
        IConfigureEndpoint EnableCommandValidators();
    }
}