using System;

namespace Iris.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EventDoesNotMutateStateAttribute : Attribute
    {
    }
}