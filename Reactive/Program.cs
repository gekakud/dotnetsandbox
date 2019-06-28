using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Reactive
{
    public class Program
    {
        static void Main(string[] args)
        {
            var dummyCommandsProducer = Observable
                .Interval(TimeSpan.FromMilliseconds(800))
                .Subscribe(c => { Console.WriteLine(c); });
            
            var messagePublisher = new PublisherService();

            Task.Run(() =>
            {
                while (true)
                {
                    //items producer
                    Thread.Sleep(200);
                    messagePublisher.PublishNewCommand(new NewMessageCommand(new Message("hello me!")));
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    //items producer
                    Thread.Sleep(500);
                    messagePublisher.PublishNewCommand(new QueryCommand("SELECT some data query"));
                }
            });

            Console.ReadKey();
            dummyCommandsProducer.Dispose();
        }
    }

    //we will publish all commands to this service
    public class PublisherService
    {
        private static Subject<AppCommand> _commandConsumer;

        //command producer passes messages to consumer
        public void PublishNewCommand(AppCommand cmd)
        {
            //items of AppCommand type consumer

            //CommandConsumer.OnError(new Exception("HAHAHA"));
            _commandConsumer.OnNext(cmd);
            //CommandConsumer.OnCompleted();
        }

        public PublisherService()
        {
            _commandConsumer = new Subject<AppCommand>();
            BindCommandHandlers();
            BindLogs();
        }

        private void BindLogs()
        {
            //log on each AppCommand
            _commandConsumer.
                Subscribe(c => { Console.WriteLine(c.Issued + "| LOGGER: got command from " + c.Issuer); });
        }

        private void BindCommandHandlers()
        {
            //define actions for two message types
            _commandConsumer.OfType<NewMessageCommand>()
                .Subscribe(
                    //onNext
                    m => { Console.WriteLine(m.Message.MsgText); },
                    //onError
                    e=>{ Console.WriteLine(" error occured" + e.Message); },
                    //onCompleted
                    (() => { Console.WriteLine("message onComplete called"); }));

            _commandConsumer.OfType<QueryCommand>()
                .Subscribe(
                q => { Console.WriteLine(q.Query + " from " + q.Issuer); });
        }
    }
}
