using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace RxNet
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Executor e = new Executor();
            e.RunExample();
        }
    }

    public class Executor
    {
        readonly List<IDisposable> subscribtionsList = new List<IDisposable>();

        public void RunExample()
        {
            var producer = new DataProducer();

            var consumer = new DataConsumer();
            var anotherConsumer = new DataConsumer();
            //Observables are data producers(data stream or sequence), they provide observers with data
            //Observers are data consumers(clients for that data). Consumer is subscribed to producer.
            producer.Subscribe(consumer);
            producer.Subscribe(anotherConsumer);

            //we create data stream provider(values of <long> each 2 seconds)
            var simpleDataStream = Observable.Interval(TimeSpan.FromSeconds(2));

            //only onNext
            IDisposable dis1 = simpleDataStream.Subscribe(
                x =>
                {
                    // Do Stuff Here
                    Console.WriteLine(x);
                    // Console WriteLine Prints
                    // 0
                    // 1
                    // 2
                    // ...
                });
            

            //IObserver implemented(onNext,onError,onCompleted)
            IDisposable dis2 = simpleDataStream.Subscribe(
                data => { Console.WriteLine("Prining your data " + data); }, //onNext
                ex => { Console.WriteLine("Error occured " + ex.Message); }, //onError
                () => { Console.WriteLine("Completed"); } //onCompleted
            );
            IDisposable dis3 = simpleDataStream.Subscribe(new DataConsumer());

            subscribtionsList.AddRange(new[] {dis1, dis2, dis3});

            var keepDoingProducer = Observable.Interval(TimeSpan.FromSeconds(2));
            var lastDis = keepDoingProducer.Subscribe(
                x =>
                {
                    Console.WriteLine("KEEP DOING STUFF...");
                });


            Thread.Sleep(12000);
            Console.WriteLine("START DISPOSING subs list");
            subscribtionsList.ForEach(d => d.Dispose());

            Console.WriteLine("Disposed.");
            Console.WriteLine("Only one data producer and consumer remain for KEEP DOING STUFF");
            Thread.Sleep(8000);
            Console.WriteLine("START DISPOSING last subscription");
            lastDis.Dispose();
            Console.WriteLine("No more activity here... press any key to exit");
            Console.ReadLine();
        }
    }

    //Observable which will create and provide 1,2,3,4,5 <int> sequence to all its subscribers
    public class DataProducer : IObservable<long>
    {
        public IDisposable Subscribe(IObserver<long> consumer)
        {
            //provide data to subscribed consumer
            //send one by one
            consumer.OnNext(1);
            consumer.OnNext(2);
            consumer.OnNext(3);
            consumer.OnNext(4);
            consumer.OnNext(5);

            //send data by range
            Observable.Range(6, 10).Subscribe(num =>
            {
                Thread.Sleep(50);
                consumer.OnNext(num);
            });

            //let consumer know that no more data will be sent
            consumer.OnCompleted();
            return Disposable.Empty;
        }
    }

    public class DataConsumer : IObserver<long>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Observable is done sending all the data.");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"Observable failed with error: {error.Message}");
        }

        public void OnNext(long value)
        {
            Console.WriteLine($"Observable got : {value}");
        }
    }
}
