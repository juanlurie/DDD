using System;

namespace Iris.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AggregateIdAttribute : Attribute
    {
    }
}