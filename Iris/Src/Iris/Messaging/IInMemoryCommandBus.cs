namespace Iris.Messaging
{
    public interface IInMemoryCommandBus
    {
        void Execute(object command);
    }
}
