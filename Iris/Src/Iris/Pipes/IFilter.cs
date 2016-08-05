using System.Collections.Generic;

namespace Iris.Pipes
{
    public interface IFilter<T>
    {
        ICollection<T> Filter(ICollection<T> input);
    }
}