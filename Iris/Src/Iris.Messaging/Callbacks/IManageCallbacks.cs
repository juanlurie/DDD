using System;
using Iris.Messaging.Pipeline;

namespace Iris.Messaging.Callbacks
{
    public interface IManageCallbacks
    {
        void HandleCallback(IncomingMessageContext context);
        ICallback SetupCallback(Guid messageId);
    }
}
