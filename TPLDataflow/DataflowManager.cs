using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using TPLDataflow.Data;

namespace TPLDataflow
{
    internal class DataflowManager : IDisposable
    {
        //responsible for processing events through complete circle of actions
        //1.receive and buffer requests
        //2.sort by priority
        //3.process request(prepare respond data - fetch it from DB or whatever...)
        //4.send respond or send error if respond failed

        private readonly ICommunicationService _communicationService;

        private const int BatchBlockSize = 5;
        private Timer _triggerTimer;

        private BatchBlock<ClientRequest> _requestBuffer;
        private TransformBlock<IEnumerable<ClientRequest>, IEnumerable<ClientRespond>> _requestProcessor;
        
        private ActionBlock<IEnumerable<ClientRespond>> _respondSender;
        private ActionBlock<IEnumerable<ClientRespond>> _errorHandler;
        private BroadcastBlock<IEnumerable<ClientRespond>> _respondBroadcastFilter;

        private readonly List<IDisposable> _disposables;
        
        public DataflowManager(ICommunicationService pCommunicationService)
        {
            _communicationService = pCommunicationService;
            _disposables = new List<IDisposable>();

            //trigger batch block each 3secs. or when filled with BatchBlockSize items
            _triggerTimer = new Timer(3000)
            {
                AutoReset = true
            };

            CreateAndLinkBlocks();
        }

        private void CreateAndLinkBlocks()
        {
            _requestBuffer = new BatchBlock<ClientRequest>(BatchBlockSize);
            
            _requestProcessor =
                new TransformBlock<IEnumerable<ClientRequest>, IEnumerable<ClientRespond>>(
                    requests => TryProcessRequests(requests));

            _respondSender =
                new ActionBlock<IEnumerable<ClientRespond>>(
                    responds =>
                    {
                        responds.Where(e => !e.IsError)
                            .ToList()
                            .ForEach(clientRespond => _communicationService.SendRespond(clientRespond));
                    });

            _errorHandler =
                new ActionBlock<IEnumerable<ClientRespond>>(
                    responds =>
                        responds.Where(e => e.IsError)
                            .ToList()
                            .ForEach(respond => Console.WriteLine(respond.RequestId + " failed")));

            _respondBroadcastFilter = new BroadcastBlock<IEnumerable<ClientRespond>>(items => items);

            _triggerTimer.Elapsed += (p_sender, p_args) => _requestBuffer.TriggerBatch();
            _triggerTimer.Start();

            _disposables.Add(_requestBuffer.LinkTo(_requestProcessor));
            _disposables.Add(_requestProcessor.LinkTo(_respondBroadcastFilter));
            _disposables.Add(_respondBroadcastFilter.LinkTo(_respondSender));
            _disposables.Add(_respondBroadcastFilter.LinkTo(_errorHandler));
        }

        private IEnumerable<ClientRespond> TryProcessRequests(IEnumerable<ClientRequest> pRequests)
        {
            _triggerTimer.Stop();
            _triggerTimer.Start();
            
            var ts = new List<Task<ClientRespond>>();

            foreach (var clientRequest in pRequests)
            {
                ts.Add(new Task<ClientRespond>(() =>
                {
                    try
                    {
                        Console.WriteLine("request " + clientRequest.ActionType + " is being processed");
                        return new ClientRespond
                        {
                            Data = clientRequest.Payload + "ready",
                            IsError = false,
                            RequestId = clientRequest.RequestId
                        };
                    }
                    catch (Exception)
                    {
                        return new ClientRespond
                        {
                            Data = string.Empty,
                            IsError = true,
                            RequestId = clientRequest.RequestId
                        };
                    }
                }));
            }

            var results = new ConcurrentBag<ClientRespond>();

            //not executed sequentially
            ts.ForEach(t =>
            {
                t.Start();
                results.Add(t.Result);
            });
            Task.WaitAll(ts.ToArray());

            return results.ToList();        
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool p_disposing)
        {
            if (p_disposing)
            {
                if (_triggerTimer != null)
                {
                    _triggerTimer.Stop();
                    _triggerTimer.Dispose();
                }
                _triggerTimer = null;

                _requestBuffer = null;
                _requestProcessor = null;
                _respondBroadcastFilter = null;
                _respondSender = null;
                _errorHandler = null;
            }
        }
    }
}