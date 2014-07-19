namespace DomainEvents.Contracts
{
    public interface IInMemoryBus
    {
        void Publish<TEvent>(TEvent message) where TEvent : IEvent;
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}