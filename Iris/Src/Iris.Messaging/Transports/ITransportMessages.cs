using System;
using Iris.Messaging.Pipeline;

namespace Iris.Messaging.Transports
{
    /// <summary>
    /// Indicates the ability to transport inbound and outbound messages.
    /// </summary>
    /// <remarks>
    /// Object instances which implement this interface must be designed to be multi-thread safe.
    /// </remarks>
    public interface ITransportMessages : IAmStartable, IDisposable
    {
        IMessageContext CurrentMessage { get; }
        void SendMessage(OutgoingMessageContext outgoingMessageContext);
        void ProcessMessage(IncomingMessageContext incomingContext);
    }
}
