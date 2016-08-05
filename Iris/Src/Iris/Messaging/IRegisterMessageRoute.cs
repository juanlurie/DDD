using System;

namespace Iris.Messaging
{
    public interface IRegisterMessageRoute
    {
        IRegisterMessageRoute RegisterRoute(Type messageType, Address endpointAddress);
    }
}