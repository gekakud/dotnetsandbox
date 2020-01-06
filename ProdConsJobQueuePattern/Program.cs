using System;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
                //adding new person(supermarket client) to a queue each interval
                simpleDataStream = Observable.Interval(TimeSpan.FromMilliseconds(50)).Subscribe(x =>
                {
                    Console.WriteLine("Client {0} joined the queue", x);
                    try
                    {
                        clientsProcessor.AddToQueue((int)x);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("!!!ERROR:" + e.Message);
                        simpleDataStream?.Dispose();
                    }

                },onError: exception =>
                {
                    Console.WriteLine(exception.Message);
                });

                //notify BlockingCollection to stop receive jobs after timeout
                Task.Delay(5000).ContinueWith(task => clientsProcessor.StopPostingJobs());

                //priority queue with custom priority IProducerConsumer implementation
//                simpleDataStream = Observable.Repeat(TimeSpan.FromMilliseconds(20),200).Subscribe(x =>
//                {
//                    messageProcessor.AddToQueue(new Message
//                    {
//                        From = "Me", To = "Me",MessageText = "Bla"
//                    },rn.Next(0,4));
//                });

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
