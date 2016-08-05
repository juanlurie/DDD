﻿using System;
using System.Reflection;
using System.Threading;
using Iris.Ioc;
using Iris.Messaging.Configuration;
using Iris.Messaging.Pipeline;
using Iris.Messaging.Pipeline.Modules;
using Iris.Pipes;

namespace Iris.Messaging.EndPoints
{
    public abstract class LocalEndpoint<TContainerBuilder> : IService
        where TContainerBuilder : IContainerBuilder, new()
    {
        private readonly Configure configuration;
        private bool disposed;

        protected LocalEndpoint()
        {
            var containerBuilder = new TContainerBuilder();
            string endpointName = Assembly.GetAssembly(GetType()).GetName().Name;

            configuration = Configure.Initialize(endpointName, containerBuilder);
            ConfigureEndpoint(configuration);
            ConfigurePipeline(containerBuilder);

            Settings.RootContainer = containerBuilder.BuildContainer();
        }

        protected abstract void ConfigureEndpoint(IConfigureEndpoint configuration);

        public IInMemoryBus Start()
        {
            configuration.Start();
            return Settings.RootContainer.GetInstance<IInMemoryBus>();
        }

        protected virtual void ConfigurePipeline(TContainerBuilder containerBuilder)
        {
            var incomingPipeline = new ModulePipeFactory<IncomingMessageContext>()
                .Add<UnitOfWorkModule>()
                .Add<DispatchMessagesModule>();

            containerBuilder.RegisterSingleton(incomingPipeline); 
        }

        public void Stop()
        {
            configuration.Stop();
        }

        ~LocalEndpoint()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Run(CancellationToken token)
        {
            configuration.Start();

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(100);
            }

            configuration.Stop();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                configuration.Stop();
            }

            disposed = true;
        }
    }
}