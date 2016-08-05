using System;
using System.ComponentModel;
using Iris.Logging;
using Iris.Messaging.Transports;
using Iris.Pipes;

namespace Iris.Messaging.Pipeline.Modules
{
    public class SendMessageModule : IModule<OutgoingMessageContext>
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(SendMessageModule));

        private readonly ISendMessages sender;
        private readonly IPublishMessages publisher;

        public SendMessageModule(ISendMessages sender, IPublishMessages publisher)
        {
            this.sender = sender;
            this.publisher = publisher;
        }

        public bool Process(OutgoingMessageContext input, Func<bool> next)
        {
            switch (input.OutgoingMessageType)
            {
                case OutgoingMessageContext.MessageType.Command:
                    SendMessage(input);
                    break;

                case OutgoingMessageContext.MessageType.Event:
                    PublishMessage(input);
                    break;

                default:
                    throw new InvalidEnumArgumentException("input.OutgoingMessageType", (int)input.OutgoingMessageType, input.OutgoingMessageType.GetType());
            }

            return next();
        }

        private void PublishMessage(OutgoingMessageContext input)
        {
            Logger.Debug("Publishing message {0}", input);
            publisher.Publish(input);
        }

        private void SendMessage(OutgoingMessageContext input)
        {
            Logger.Debug("Sending message {0} to {1}", input, input.Destination);
            sender.Send(input.GetTransportMessage(), input.Destination);
        }
    }
}