﻿using System;
using System.Runtime.Serialization;

namespace Iris.Persistence
{
    [Serializable]
    public class UnitOfWorkRollbackException : Exception
    {
        public UnitOfWorkRollbackException()
        {
        }

        public UnitOfWorkRollbackException(string message)
            : base(message)
        {
        }

        public UnitOfWorkRollbackException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected UnitOfWorkRollbackException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}