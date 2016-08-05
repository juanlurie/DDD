using System;
using Iris.Logging;
using Iris.Messaging.Serialization;
using Iris.Pipes;

namespace Iris.Messaging.Pipeline.Modules
{
    public class MessageSerializationModule : IModule<OutgoingMessageContext>
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(MessageSerializationModule));
        private readonly ISerializeMessages messageSerializer;

        public MessageSerializationModule(ISerializeMessages messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public bool Process(OutgoingMessageContext input, Func<bool> next)
        {
            Logger.Debug("Serializing message body for message {0}", input);
            input.MessageSerializationFunction(SerializeMessages);
            return next();
        }

        private byte[] SerializeMessages(object message)
        {
            if (message != null)
            {
                return messageSerializer.Serialize(message);
            }
             
            return new byte[0];
        }        
    }
}