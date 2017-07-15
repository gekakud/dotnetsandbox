using System;
using System.Collections.Generic;

namespace TPLDataflow
{
    public class DummyActionsGenerator : ICommunicationService
    {
        private static List<ClientRequest> GetServerActions()
        {
            var list = new List<ClientRequest>();
            var rnd = new Random(DateTime.Now.Millisecond);
            var numOfActions = rnd.Next(1, 5);

            for (var i = 0; i < numOfActions; i++)
            {
                var randomRequestType = (TypeOfClientRequest) rnd.Next(0, (int) TypeOfClientRequest.Find);

                list.Add(new ClientRequest
                {
                    ActionType = randomRequestType,
                    Payload = "get some data",
                    RequestId = Guid.NewGuid()
                });
            }

            return list;
        }

        public bool SendRespond(ClientRespond p_respond)
        {
            return true;
        }

        public List<ClientRequest> GetRequests()
        {
            return GetServerActions();
        }
    }
}