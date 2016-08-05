﻿using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Iris.Pipes
{
    public class ModulePipe<T>
    {
        private readonly Queue<Type> chain;
        private readonly IServiceLocator serviceLocator;

        public ModulePipe(Queue<Type> chain, IServiceLocator serviceLocator)
        {
            this.chain = chain;
            this.serviceLocator = serviceLocator;
        }

        public virtual bool Invoke(T input)
        {
            return InvokeNext(input);
        }

        protected virtual bool InvokeNext(T input)
        {
            if (chain.Count == 0)
            {
                return true;
            }

            var processor = (IModule<T>)serviceLocator.GetInstance(chain.Dequeue());

            return TryInvokeNext(input, processor);
        }

        private bool TryInvokeNext(T input, IModule<T> processor)
        {
            var result = processor.Process(input, () => InvokeNext(input));
            return result;
        }
    }
}