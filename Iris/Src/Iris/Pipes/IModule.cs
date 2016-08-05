using System;

namespace Iris.Pipes
{
    public interface IModule<in T>
    {
        bool Process(T input, Func<bool> next);
    }
}