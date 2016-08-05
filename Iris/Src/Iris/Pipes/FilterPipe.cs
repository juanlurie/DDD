﻿using System.Collections.Generic;
using System.Linq;

namespace Iris.Pipes
{
    public abstract class FilterPipe<T>
    {
        private readonly List<IFilter<T>> filters = new List<IFilter<T>>();

        public FilterPipe<T> Register(IFilter<T> filter)
        {
            filters.Add(filter);
            return this;
        }

        public virtual ICollection<T> Filter(ICollection<T> values)
        {
            T[] current = values.ToArray();

            foreach (IFilter<T> filer in filters)
            {
                current = filer.Filter(current).ToArray();
            }

            return current;
        }
    }
}
