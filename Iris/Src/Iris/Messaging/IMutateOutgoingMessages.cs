namespace Iris.Messaging
{
    public interface IMutateOutgoingMessages
    {
        object Mutate(object message);
    }
}