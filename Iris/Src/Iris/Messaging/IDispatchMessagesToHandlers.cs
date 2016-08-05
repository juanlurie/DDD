using Microsoft.Practices.ServiceLocation;

namespace Iris.Messaging
{
    public interface IDispatchMessagesToHandlers
    {
        void DispatchToHandlers(object message, IServiceLocator serviceLocator);
    }
}