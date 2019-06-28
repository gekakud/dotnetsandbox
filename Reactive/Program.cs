using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
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
            
            var ss = new SomeService();

            Task.Run(() =>
            {
                while (true)
                {
                    //items producer
                    Thread.Sleep(200);
                    ss.PublishNewCommand(new NewMessageCommand(new Message("hello me!")));
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    //items producer
                    Thread.Sleep(500);
                    ss.PublishNewCommand(new QueryCommand("SELECT some data query"));
                }
            });

            Console.ReadKey();
            dummyCommandsProducer.Dispose();
        }

        
    }

    public class SomeService
    {
        private static Subject<AppCommand> InnerCommands;

        public static IObservable<AppCommand> Commands
        {
            get { return InnerCommands; }
        }

        public void PublishNewCommand(AppCommand cmd)
        {
            //items of AppCommand type consumer

            //InnerCommands.OnError(new Exception("HAHAHA"));
            InnerCommands.OnNext(cmd);
            //InnerCommands.OnCompleted();
        }

        public SomeService()
        {
            InnerCommands = new Subject<AppCommand>();
            BindCommandHandlers();
            BindLogs();
        }

        private void BindLogs()
        {
            //log on each AppCommand
            Commands.Subscribe(c => { Console.WriteLine(c.Issued + "| LOGGER: got command from " + c.Issuer); });
        }

        private void BindCommandHandlers()
        {
            Commands.OfType<NewMessageCommand>()
                .Subscribe(
                    //onNext
                    m => { Console.WriteLine(m.Message.MsgText); },
                    //onError
                    e=>{ Console.WriteLine(" error occured" + e.Message); },
                    //onCompleted
                    (() => { Console.WriteLine("message onComplete called"); }));

            Commands.OfType<QueryCommand>()
                .Subscribe(
                q => { Console.WriteLine(q.Query + " from " + q.Issuer); });
        }
    }
}
