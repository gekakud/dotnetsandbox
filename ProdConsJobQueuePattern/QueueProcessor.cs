using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ProdConsJobQueuePattern
{
    /// <summary>
    /// Managing worker threads and thread-safe queue with blocking collection
    /// </summary>
    public class QueueProcessor
    {
        //using BlockingCollection to avoid polling
        private readonly BlockingCollection<int>
            _clientsQueue = new BlockingCollection<int>(new ConcurrentQueue<int>());

        //create independent cashiers(threads)
        public QueueProcessor(int numThreads)
        {
            for (var i = 0; i < numThreads; i++)
            {
                var thread = new Thread(OnHandlerStart)
                    { IsBackground = true };
                thread.Start();
            }
        }

        public void AddToQueue(int job)
        {
            _clientsQueue.Add(job);
        }

        private async void OnHandlerStart()
        {
            var rn = new Random(DateTime.Now.Millisecond);

            while (true)
            {
                //blocking thread until there is new client to process
                var currentClienId = _clientsQueue.Take();
                var processingDelay = rn.Next(1, 5);

                //processing...
                await Task.Delay(processingDelay * 1000);

                Console.WriteLine("Cashier {0} finished processing client {1} within {2} seconds"
                    , Thread.CurrentThread.ManagedThreadId
                    , currentClienId, processingDelay);
            }
        }

        public void Dispose()
        {
            _clientsQueue?.Dispose();
        }
    }
}
