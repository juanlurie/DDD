using System;

namespace Iris.Messaging
{
    public interface IMessageContext 
    {
        Guid MessageId { get; }
        Guid CorrelationId { get; }
        string UserName { get; }
        Address ReplyToAddress { get; }
    }
}