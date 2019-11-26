using System;
using System.Reactive.Linq;

namespace ProdConsJobQueuePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientsProcessor = new QueueProcessor(5);

            //adding new person(supermarket client) to a queue each second
            var simpleDataStream = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(x =>
            {
                Console.WriteLine("Client {0} joined the queue", x);
                clientsProcessor.AddClientToQueue((int)x);
            });

            Console.ReadKey();
            simpleDataStream.Dispose();
            clientsProcessor.Dispose();
        }
    }
}
