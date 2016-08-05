namespace Iris.Messaging
{
    public interface IInMemoryEventBus
    {
        void Raise(object @event);
    }
}