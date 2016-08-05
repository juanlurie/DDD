using System;
using System.Collections.Generic;
using System.Transactions;
using Iris.Ioc;
using Iris.Logging;
using Iris.Messaging.Transports;
using Iris.Pipes;
using Microsoft.Practices.ServiceLocation;

namespace Iris.Messaging.Pipeline
{
    public class IncomingMessageContext : IMessageContext, IEquatable<IMessageContext>, IEquatable<IncomingMessageContext>
    {
        public static readonly ILog Logger = LogFactory.BuildLogger(typeof(IncomingMessageContext));
        public TransportMessage TransportMessage { get; private set; }
        public bool IsLocalMessage { get; private set; }
        public IServiceLocator ServiceLocator { get; private set; }

        public static IMessageContext Null { get; private set; }

        private readonly Guid messageId;

        static IncomingMessageContext()
        {
            Null = new IncomingMessageContext
            {
                ServiceLocator = new DisposedProvider(),
                TransportMessage = TransportMessage.Undefined,
                Message = new object()
            };
        }

        public object Message { get; protected set; }

        public Guid MessageId
        {
            get { return messageId; }
        }

        public Guid CorrelationId
        {
            get { return TransportMessage.CorrelationId; }
        }

        public Address ReplyToAddress
        {
            get { return TransportMessage.ReplyToAddress; }
        }

        public string UserName
        {
            get { return GetUserName(); }
        }

        protected IncomingMessageContext()
        {
        }

        public IncomingMessageContext(TransportMessage transportMessage, IServiceLocator serviceLocator)
        {
            TransportMessage = transportMessage;
            messageId = transportMessage.MessageId;
            ServiceLocator = serviceLocator;
        }

        public IncomingMessageContext(object localMessage, IServiceLocator serviceLocator)
        {
            SetMessage(localMessage);
            IsLocalMessage = true;
            ServiceLocator = serviceLocator;
            messageId = SequentialGuid.New();

            TransportMessage = new TransportMessage(messageId, messageId, Address.Local, TimeSpan.MaxValue, new Dictionary<string, string>(), new byte[0]);
        }

        public void Process(ModulePipeFactory<IncomingMessageContext> incomingPipeline)
        {
            using (var scope = StartTransactionScope())
            {
                var pipeline = incomingPipeline.Build(ServiceLocator);
                pipeline.Invoke(this);
                Logger.Debug("Committing Transaction Scope");
                scope.Complete();
            }
        }

        protected virtual TransactionScope StartTransactionScope()
        {
            Logger.Debug("Beginning a transaction scope with option[Suppress]");
            return TransactionScopeUtils.Begin(TransactionScopeOption.Suppress);
        }

        public string GetUserName()
        {
            if (TransportMessage.Headers.ContainsKey(HeaderKeys.UserName))
            {
                return TransportMessage.Headers[HeaderKeys.UserName];
            }

            return String.Empty;
        }

        public bool IsControlMessage()
        {
            return TransportMessage.Headers.ContainsKey(HeaderKeys.ControlMessageHeader);
        }

        public void SetMessage(object message)
        {
            Mandate.ParameterNotNull(message, "message");

            Message = message;
        }

        public override int GetHashCode()
        {
            return MessageId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IncomingMessageContext);
        }

        public virtual bool Equals(IMessageContext other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.MessageId.Equals(MessageId);
            }

            return false;
        }

        public bool Equals(IncomingMessageContext other)
        {
            return Equals((IMessageContext)other);
        }

        public static bool operator ==(IncomingMessageContext left, IncomingMessageContext right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IncomingMessageContext left, IncomingMessageContext right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return TransportMessage.MessageId.ToString();
        }
    }
}