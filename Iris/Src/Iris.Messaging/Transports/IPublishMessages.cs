using Iris.Messaging.Pipeline;

namespace Iris.Messaging.Transports
{
    public interface IPublishMessages
    {
        bool Publish(OutgoingMessageContext outgoingMessage);
    }
}
