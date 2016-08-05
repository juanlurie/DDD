using System;

namespace Iris.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}