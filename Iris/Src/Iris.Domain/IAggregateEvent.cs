using Iris.Messaging;

namespace Iris.Domain
{
    public interface IAggregateEvent : IDomainEvent
    {
        int Version { get; }
    }
}