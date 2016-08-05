using Iris.Ioc;
using Iris.Logging;
using Iris.Messaging.Pipeline;
using Iris.Pipes;

namespace Iris.Messaging.Transports
{
    public class Transport : ITransportMessages
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(Transport)); 

        private readonly ModulePipeFactory<IncomingMessageContext> incomingPipeline;

        private readonly WebSafeThreadLocal<IMessageContext> currentMessageBeingProcessed = new WebSafeThreadLocal<IMessageContext>();

        public IMessageContext CurrentMessage
        {
            get
            {
                return currentMessageBeingProcessed.Value ?? IncomingMessageContext.Null;
            }
        }

        public Transport(ModulePipeFactory<IncomingMessageContext> incomingPipeline)
        {
            this.incomingPipeline = incomingPipeline;
        }

        public void Dispose()
        {
            Logger.Debug("Dispose called; stopping message receiver.");
        }
    
        public virtual void ProcessMessage(IncomingMessageContext incomingContext)
        {
            try
            {
                currentMessageBeingProcessed.Value = incomingContext;
                incomingContext.Process(incomingPipeline);
            }
            finally
            {
                currentMessageBeingProcessed.Value = IncomingMessageContext.Null;
            }
        }
    }
}
