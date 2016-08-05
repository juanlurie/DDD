using System;

namespace Iris.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UnitOfWorkCommitOrderAttribute : Attribute
    {
        public int Order { get; set; }
    }
}