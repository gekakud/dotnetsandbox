using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace TPLDataflow
{
    internal class DataflowManager
    {
        //responsible for processing events through complete circle of actions
        //1.receive and buffer requests
        //2.sort by priority
        //3.process request(prepare respond data - fetch it from DB or whatever...)
        //4.send respond or send error if respond failed

        private readonly ICommunicationService _communicationService;
        
        private BatchBlock<ClientRequest> _requestBuffer;
        private TransformBlock<IEnumerable<ClientRequest>, IEnumerable<ClientRespond>> _requestProcessor;
        private ActionBlock<IEnumerable<ClientRespond>> _respondSender;

        private readonly List<IDisposable> _disposables;

        public DataflowManager(ICommunicationService pCommunicationService)
        {
            _communicationService = pCommunicationService;
            _disposables = new List<IDisposable>();

            CreateAndLinkBlocks();
        }

        private void CreateAndLinkBlocks()
        {
            _requestBuffer = new BatchBlock<ClientRequest>(5);
            _requestProcessor =
                new TransformBlock<IEnumerable<ClientRequest>, IEnumerable<ClientRespond>>(
                    requests => TryProcessRequests(requests));

            _respondSender = new ActionBlock<IEnumerable<ClientRespond>>(responds =>
            {
                foreach (var clientRespond in responds)
                {
                    _communicationService.SendRespond(clientRespond);
                }
            });

            _disposables.Add(_requestBuffer.LinkTo(_requestProcessor));
            _disposables.Add(_requestProcessor.LinkTo(_respondSender));
        }

        private IEnumerable<ClientRespond> TryProcessRequests(IEnumerable<ClientRequest> pRequests)
        {
            //not executed sequentially
            return pRequests.AsParallel().Select(p =>
            {
                Console.WriteLine("request " + p.ActionType + " is being processed");
                try
                {
                    return new ClientRespond
                    {
                        Data = p.Payload + "ready",
                        IsError = false,
                        RequestId = p.RequestId
                    };
                }
                catch (Exception)
                {
                    return new ClientRespond
                    {
                        Data = string.Empty,
                        IsError = true,
                        RequestId = p.RequestId
                    };
                }
            });
        }

        public void PostRequestsToBuffer()
        {
            var requests = _communicationService.GetRequests();
            if (requests!=null && requests.Any())
            {
                Console.WriteLine("Got " + requests.Count + " requests");
                foreach (var clientRequest in requests)
                {
                    _requestBuffer.Post(clientRequest);
                }
            }
        }
    }
}