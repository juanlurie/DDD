﻿using System;
using Hermes.EntityFramework;
using Hermes.Logging;
using Hermes.Messaging;
using Hermes.Messaging.Configuration;
using Hermes.Messaging.EndPoints;
using Hermes.Messaging.Transports.SqlTransport;
using Hermes.ObjectBuilder.Autofac;
using Hermes.Serialization.Json;

using IntegrationTest.Contracts;
using IntegrationTests.PersistenceModel;

namespace IntegrationTest.Client
{
    public class RequestorEndpoint : LocalEndpoint<AutofacAdapter>
    {
        protected override void ConfigureEndpoint(IConfigureEndpoint configuration)
        {
            LogFactory.BuildLogger = t => new ConsoleWindowLogger(t);
            ConsoleWindowLogger.MinimumLogLevel = LogLevel.Info;

            configuration
                .UseJsonSerialization()
                .UseSqlTransport()
                .DefineCommandAs(IsCommand)
                .DefineEventAs(IsEvent)
                .UserNameResolver(() => "SomeUser")
                .RegisterMessageRoute<AddRecordToDatabase>(Address.Parse("IntegrationTest"))
                .SendOnlyEndpoint()
                .ConfigureEntityFramework<IntegrationTestContext>("IntegrationTest");
        }

        private static bool IsCommand(Type type)
        {
            return typeof(ICommand).IsAssignableFrom(type);
        }

        private static bool IsEvent(Type type)
        {
            return typeof(IEvent).IsAssignableFrom(type);
        }
    }    
}
