using System;

namespace Iris.Persistence
{
    public interface ITimestampPersistenceAudit
    {
        DateTime ModifiedTimestamp { get; set; }
        DateTime CreatedTimestamp { get; set; }
    }
}