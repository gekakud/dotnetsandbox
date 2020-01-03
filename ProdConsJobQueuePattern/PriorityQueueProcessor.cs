using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProdConsJobQueuePattern
{
    public class Message
    {
        public string MessageText { get; set; }
        public string To { get; set; }
        public string From { get; set; }
    }

    /// <summary>
    /// Elements with higher priority will be handled first
    /// </summary>
    public class PriorityQueueProcessor:IDisposable
    {
        private readonly BlockingCollection<KeyValuePair<int, Message>> _messageQueue;

        public PriorityQueueProcessor(int numOfThreads)
        {
            //set number of possible priorities range - from 0 to 4
            var queueImpl = new PriorityQueueImpl<int, Message>(5);
            _messageQueue = new BlockingCollection<KeyValuePair<int, Message>>(queueImpl);

            for (int i = 0; i < numOfThreads; i++)
            {
                var t = new Thread(OnHandlerStart)
                {
                    IsBackground = true
                };

                t.Start();
            }
        }

        private async void OnHandlerStart()
        {
            var rn = new Random(DateTime.Now.Millisecond);

            while (true)
            {
                //blocking thread until there is new client to process
                var currentMessagePair = _messageQueue.Take();
                var processingDelay = rn.Next(1, 5);

                //processing...
                await Task.Delay(processingDelay * 100);

                Console.WriteLine("Message {0} with priority {1} was sent by thread {2}",
                    currentMessagePair.Value, currentMessagePair.Key, Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void AddToQueue(Message m, int prio)
        {
            _messageQueue.Add(new KeyValuePair<int, Message>(prio,m));
        }

        public void Dispose()
        {
            _messageQueue?.Dispose();
        }
    }
}