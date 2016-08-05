using System;
using Iris.Messaging.Management;
using Iris.Queries;

namespace Iris.Messaging
{
    public interface IManageErrorQueues
    {
        int GetErrorCount();
        void Delete(Guid id);
        void Resend(TransportMessageDto dto);
        PagedResult<TransportMessageDto> GetErrorMessages(int pageNumber, int resultsPerPage);
    }
}
