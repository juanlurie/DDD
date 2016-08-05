namespace Iris.Messaging
{
    public interface IDequeueMessages
    {
        TransportMessage Dequeue();
    }
}