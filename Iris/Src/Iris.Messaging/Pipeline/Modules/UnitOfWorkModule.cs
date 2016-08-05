﻿using System;
using System.Collections.Generic;
using System.Linq;
using Iris.Attributes;
using Iris.Extensions;
using Iris.Logging;
using Iris.Persistence;
using Iris.Pipes;

namespace Iris.Messaging.Pipeline.Modules
{
    public class UnitOfWorkModule : IModule<IncomingMessageContext>
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(UnitOfWorkModule));
        private readonly ICollection<IUnitOfWork> unitsOfWork;

        public UnitOfWorkModule(IEnumerable<IUnitOfWork> unitsOfWork)
        {
            this.unitsOfWork = unitsOfWork.ToArray();
        }

        public bool Process(IncomingMessageContext input, Func<bool> next)
        {
            try
            {
                var result = next();
                CommitUnitsOfWork(input);                
                return result;
            }
            catch (Exception ex)
            {
                SytemCircuitBreaker.Trigger(ex);
                RollBackUnitsOfWork(input);
                input.TransportMessage.Headers[HeaderKeys.FailureDetails] = ex.GetFullExceptionMessage();
                throw;
            }
        }

        private void CommitUnitsOfWork(IncomingMessageContext input)
        {
            foreach (var unitOfWork in OrderedUnitsOfWork())
            {
                Logger.Debug("Committing {0} unit-of-work for message {1}", unitOfWork.GetType().FullName, input);
                unitOfWork.Commit();
            }
        }

        private void RollBackUnitsOfWork(IncomingMessageContext input)
        {
            foreach (var unitOfWork in OrderedUnitsOfWork().Reverse())
            {
                Logger.Warn("Rollback of {0} unit-of-work for message {1}", unitOfWork.GetType().FullName, input);
                unitOfWork.Rollback();
            }
        }

        private IEnumerable<IUnitOfWork> OrderedUnitsOfWork()
        {
            return unitsOfWork
                .Where(something => something.HasAttribute<InitializationOrderAttribute>())
                .OrderBy(i => i.GetCustomAttributes<InitializationOrderAttribute>().First().Order)
                .Union(unitsOfWork.Where(i => !i.HasAttribute<InitializationOrderAttribute>()));
        }
    }
}