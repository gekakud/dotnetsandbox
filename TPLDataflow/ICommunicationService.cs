using System.Collections.Generic;

namespace TPLDataflow
{
    public interface ICommunicationService
    {
        bool SendRespond(ClientRespond p_respond);
        List<ClientRequest> GetRequests();
    }
}