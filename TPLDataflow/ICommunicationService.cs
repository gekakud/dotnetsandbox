using System.Collections.Generic;
using TPLDataflow.Data;

namespace TPLDataflow
{
    public interface ICommunicationService
    {
        bool SendRespond(ClientRespond p_respond);
        List<ClientRequest> GetRequests();
    }
}