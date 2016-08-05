using System.Collections.Generic;

namespace Iris.Domain
{
    public interface IAggregate : IEntity
    {
        IEnumerable<IAggregateEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
        int GetVersion();
        void LoadFromHistory(IEnumerable<IAggregateEvent> domainEvents);
    }
}