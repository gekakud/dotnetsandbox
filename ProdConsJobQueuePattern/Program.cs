using System;
using System.Reactive.Linq;
using System.Security.Cryptography;

namespace ProdConsJobQueuePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientsProcessor = new QueueProcessor(5);
            var messageProcessor = new PriorityQueueProcessor(5);

            var rn = new Random(DateTime.Now.Millisecond);
            IDisposable simpleDataStream = null;
            try
            {
                //simple job queue with BlockingCollection
                //adding new person(supermarket client) to a queue each second
                //                simpleDataStream = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>
                //                {
                //                    Console.WriteLine("Client {0} joined the queue", x);
                //                    clientsProcessor.AddToQueue((int)x);
                //                });

                //priority queue with custom priority IProducerConsumer implementation
                simpleDataStream = Observable.Interval(TimeSpan.FromMilliseconds(20)).Subscribe(x =>
                {
                    messageProcessor.AddToQueue(new Message
                    {
                        From = "Me", To = "Me",MessageText = "Bla"
                    },rn.Next(0,4));
                });


                Console.ReadKey();
            }
            finally
            {
                simpleDataStream?.Dispose();
                clientsProcessor.Dispose();
                messageProcessor.Dispose();
            }
        }
    }
}
