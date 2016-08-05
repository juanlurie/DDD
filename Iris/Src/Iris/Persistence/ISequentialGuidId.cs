using System;

namespace Iris.Persistence
{
    public interface ISequentialGuidId
    {
        Guid Id { get; set; }
    }
}