namespace DomainEvents.Contracts
{
    public interface IHandle<in T>
    {
        void Handle(T @event);
    }
}