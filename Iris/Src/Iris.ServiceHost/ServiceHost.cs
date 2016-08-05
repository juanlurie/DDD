﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Iris.Logging;
using Iris.Reflection;

namespace Iris.ServiceHost
{
    class ServiceHost
    {
        static readonly ILog Logger = LogFactory.BuildLogger(typeof(ServiceHost));

        private readonly Type serviceType;
        private readonly object syncLock = new object();
        private CancellationTokenSource tokenSource;        

        public ServiceHost(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        public void Start()
        {
            lock (syncLock)
            {
                if (tokenSource != null && !tokenSource.IsCancellationRequested)
                {
                    Logger.Warn("Aborting service-host start request as the service is already running.");
                    return;
                }
                
                RunServices();
            }
        }

        private void RunServices()
        {
            tokenSource = new CancellationTokenSource();

            try
            {
                Logger.Info("Starting service {0}", serviceType.FullName);
                var service = (IService)ObjectFactory.CreateInstance(serviceType);
                Task.Factory.StartNew(() => service.Run(tokenSource.Token), tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            }
            catch
            {
                tokenSource.Cancel();
                throw;
            }
        }

        public void Stop()
        {
            lock (syncLock)
            {
                Logger.Info("Sending termination signal to services.");
                tokenSource.Cancel();
            }
        }
    }
}