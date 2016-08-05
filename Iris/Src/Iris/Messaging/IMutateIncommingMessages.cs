namespace Iris.Messaging
{
    public interface IMutateIncomingMessages
    {
        object Mutate(object message);
    }
}