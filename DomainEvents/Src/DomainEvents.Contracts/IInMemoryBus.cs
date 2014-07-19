namespace DomainEvents.Contracts
{
    public interface IInMemoryBus
    {
        void Publish(IEvent message);
        void Send(ICommand command);
    }
}