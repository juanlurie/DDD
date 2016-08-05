﻿using System;
using Iris.Logging;
using Iris.Messaging.Configuration;
using Iris.Pipes;

namespace Iris.Messaging.Pipeline.Modules
{
    public class AuditModule : IModule<IncomingMessageContext>
    {
        private readonly static ILog Logger = LogFactory.BuildLogger(typeof(AuditModule));

        private readonly ISendMessages messageSender;

        public AuditModule(ISendMessages messageSender)
        {
            this.messageSender = messageSender;
        }

        public bool Process(IncomingMessageContext input, Func<bool> next)
        {
            DateTime receivedTime = DateTime.UtcNow;

            if (input.IsLocalMessage)
                return next();

            if (next())
            {
                ProcessCompletedHeaders(input.TransportMessage, receivedTime);
                SendToAuditQueue(input.TransportMessage);
                return true;
            }

            return false;
        }

        private void SendToAuditQueue(TransportMessage transportMessage)
        {
            if (Settings.DisableMessageAudit)
                return;

            try
            {
                Logger.Debug("Sending message {0} to audit queue", transportMessage.MessageId);
                messageSender.Send(transportMessage, Settings.AuditEndpoint);
            }
            catch
            {
                RemoveCompletedHeaders(transportMessage);
                throw;
            }
        }

        private static void RemoveCompletedHeaders(TransportMessage transportMessage)
        {
            transportMessage.Headers.Remove(HeaderKeys.CompletedTime);
            transportMessage.Headers.Remove(HeaderKeys.ReceivedTime);
            transportMessage.Headers.Remove(HeaderKeys.ProcessingEndpoint);
        }

        private void ProcessCompletedHeaders(TransportMessage transportMessage, DateTime receivedTime)
        {
            transportMessage.Headers.Remove(HeaderKeys.TimeoutExpire);
            transportMessage.Headers.Remove(HeaderKeys.RouteExpiredTimeoutTo);
            transportMessage.Headers.Remove(HeaderKeys.FailureDetails);

            transportMessage.Headers[HeaderKeys.ProcessingEndpoint] = Address.Local.ToString();
            transportMessage.Headers[HeaderKeys.ReceivedTime] = receivedTime.ToWireFormattedString();
            transportMessage.Headers[HeaderKeys.CompletedTime] = DateTime.UtcNow.ToWireFormattedString();
        }
    }
}