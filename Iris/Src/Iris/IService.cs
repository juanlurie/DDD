using System;
using System.Threading;

namespace Iris
{
    public interface IService : IDisposable
    {
        void Run(CancellationToken token);
    }
}