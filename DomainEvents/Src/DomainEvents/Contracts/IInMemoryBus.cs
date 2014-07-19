namespace DomainEvents.Contracts
{
    public interface IInMemoryBus
    {
        void Publish(object @event);
    }
}