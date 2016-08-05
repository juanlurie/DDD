using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Iris.Messaging.Transports;
using Iris.Pipes;
using Microsoft.Practices.ServiceLocation;

namespace Iris.Messaging.Pipeline
{
    public class OutgoingMessageContext
    {
        private readonly List<HeaderValue> messageHeaders = new List<HeaderValue>();
        private readonly Guid messageId;
        private object outgoingMessage;
        private Guid correlationId;
        private TimeSpan timeToLive = TimeSpan.MaxValue;
        private Address replyToAddress = Address.Local;
        private Address destination = Address.Undefined;
        private Func<object, byte[]> serializeBodyFunction;
        private Func<OutgoingMessageContext, Dictionary<string, string>> buildHeaderFunction;
        
        public MessageType OutgoingMessageType { get; protected set; }

        public IServiceLocator ServiceLocator { get; protected set; }

        public TimeSpan TimeToLive { get { return timeToLive; } }

        public Guid MessageId
        {
            get { return messageId; }
        }

        public object OutgoingMessage { get { return outgoingMessage; } }

        public Guid CorrelationId
        {
            get { return correlationId == Guid.Empty ? MessageId : correlationId; }
        }

        public Address ReplyToAddress
        {
            get { return replyToAddress; }
        }

        public Address Destination
        {
            get { return destination; }
        }

        public IEnumerable<HeaderValue> Headers
        {
            get { return messageHeaders; }
        }

        protected OutgoingMessageContext()
        {
            messageId = SequentialGuid.New();
        }

        public static OutgoingMessageContext BuildCommand(Address address, Guid correlationId, TimeSpan timeToLive, object message)
        {
            MessageRuleValidation.ValidateCommand(message);

            var context = new OutgoingMessageContext
            {
                correlationId = correlationId,
                OutgoingMessageType = MessageType.Command,
                outgoingMessage = message,
                destination = address,
                timeToLive = timeToLive
            };

            return context;
        }

        public static OutgoingMessageContext BuildEvent(Guid correlationId, object message)
        {
            MessageRuleValidation.ValidateEvent(message);

            var context = new OutgoingMessageContext
            {
                correlationId = correlationId,
                OutgoingMessageType = MessageType.Event,
                outgoingMessage = message
            };

            return context;
        }

        public void Process(ModulePipeFactory<OutgoingMessageContext> outgoingPipeline, IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
            var pipeline = outgoingPipeline.Build(serviceLocator);
            pipeline.Invoke(this);
        }

        public void SetReplyAddress(Address address)
        {
            Mandate.That(address != Address.Undefined, String.Format("It is not possible to send a message to {0}", address));
            replyToAddress = address;
        }

        public void AddHeader(HeaderValue headerValue)
        {
            messageHeaders.Add(headerValue);
        }

        public ICollection<Type> GetMessageContracts()
        {
            if(outgoingMessage == null)
                return new Type[0];

            return outgoingMessage.GetContracts();
        }

        public string GetMessageContractsString()
        {
            var contracts = GetMessageContracts().Select(type => type.FullName.ToString(CultureInfo.InvariantCulture));
            return String.Join("; ", contracts);
        }

        public TransportMessage GetTransportMessage()
        {
            var body = serializeBodyFunction(OutgoingMessage);
            var headers = buildHeaderFunction(this);

            return new TransportMessage(MessageId, CorrelationId, ReplyToAddress, TimeToLive, headers, body);
        }

        public void MessageSerializationFunction(Func<object, byte[]> serializeBody)
        {
            serializeBodyFunction = serializeBody;
        }

        public void BuildHeaderFunction(Func<OutgoingMessageContext, Dictionary<string, string>> buildHeader)
        {
            buildHeaderFunction = buildHeader;
        }

        public override string ToString()
        {
            return String.Format("{0} : {1}", messageId, GetMessageContractsString());
        }

        public enum MessageType
        {
            Unknown,
            Command,
            Event
        }
    }
}