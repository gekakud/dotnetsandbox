using System;
using System.Threading;

namespace Delegates
{
    class Program
    {
        //delegates are also reference types like classes,
        //except they hold references to other methods
        //different types of delegates: delegate, Action, Func, Predicate
        delegate string SimpleDelegate(DateTime val);

        //delegateExample of type SimpleDelegate is a reference to DelegateExample method
        static SimpleDelegate delegateExample = DelegateExample;

        private static string DelegateExample(DateTime val)
        {
            return val.ToShortDateString();
        }

        static Action act = () => { Console.WriteLine("Inside action lambda"); };
        private static Predicate<int> pred = d =>
        {
            return d > 0;
        };

        //receives 3 params
        static Action<string, int, int> act3 = SumAct;

        //Func can never return void, it will always require at least one type argument
        private static Func<DateTime, string> dateToString
            = time => { return time.ToShortDateString(); };

        private static void SumAct(string text, int num1, int num2)
        {
            var sum = num1 + num2;
            Console.WriteLine(text + "\n...processing...");
            Thread.Sleep(500);
            Console.WriteLine("sum={0} processed on {1} thread",sum, Thread.CurrentThread.ManagedThreadId);
        }

        static void Main(string[] args)
        {
            var currentTime = delegateExample(DateTime.Now);
            Console.WriteLine(currentTime);

            dateToString(DateTime.Now);
            act();

            act3("no invoke used", 22, 100);

            //Executes synchronously, on the same thread
            act3.Invoke("act running SYNChronously",5, 3);

            //Executes asynchronously, on a threadpool thread (a thread taken from threadpool)
            IAsyncResult result = act3.BeginInvoke("act running ASYNChronously",6, 5, ar =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("completion callback received " + ar.IsCompleted);
            },null);

            Console.WriteLine("Continue processing on main thread");
            Console.WriteLine("Main thread {0} is finished", Thread.CurrentThread.ManagedThreadId);
            Console.ReadKey();
        }
    }
}