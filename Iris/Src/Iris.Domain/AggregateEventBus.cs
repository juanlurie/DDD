using Iris.Ioc;
using Iris.Messaging;

namespace Iris.Domain
{
    internal static class AggregateEventBus
    {
        public static void Raise(IAggregateEvent e)
        {
            var eventBus = ServiceLocator.Current.GetInstance<IInMemoryEventBus>();
            eventBus.Raise(e);
        }
    }
}