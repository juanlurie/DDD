namespace Iris.Domain
{
    public interface IEntity : IAmRestorable
    {
        IIdentity Identity { get; }
        bool ApplyEvent(IAggregateEvent @event);
    }
}