using System;
using System.Collections.Generic;
using System.Linq;

namespace Iris.Extensions
{
    public static class ObjectExtensions
    {
        public static bool HasAttribute<TAttribute>(this object o) where TAttribute : Attribute
        {
            return Attribute.IsDefined(o.GetType(), typeof(TAttribute));
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this object o) where TAttribute : Attribute
        {
            return o.GetType().GetCustomAttributes(typeof (TAttribute), true).Cast<TAttribute>();
        }
    }
}