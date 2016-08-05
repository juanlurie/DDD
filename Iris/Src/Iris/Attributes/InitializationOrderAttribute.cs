using System;

namespace Iris.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InitializationOrderAttribute : Attribute
    {
        public int Order { get; set; }
    }
}