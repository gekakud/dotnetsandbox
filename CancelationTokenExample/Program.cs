using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancelationTokenExample
{
    internal class Program
    {
        private static void Main()
        {
            var cts = new CancellationTokenSource();

            //inform task and TPL that we have cancellation token
            var t1 = Task.Factory.StartNew(MyTask, cts.Token, cts.Token);
            try
            {

                Thread.Sleep(2000);

                //Request to cancel execution of MyTask
                cts.Cancel();
                t1.Wait();
            }
            catch (Exception)
            {
                if (t1.IsCanceled)
                {
                    Console.WriteLine("Back to Main Task: MyTask NOT finished...");
                }
            }
            finally
            {
                t1.Dispose();
                cts.Dispose();
            }

            Console.WriteLine("Main Task finished...");
            Console.ReadKey();
        }

        private static void MyTask(object p_ct)
        {
            var ctCancelationToken = (CancellationToken)p_ct;

            Console.WriteLine("MyTask started running...");

            for (var i = 0; i < 10; i++)
            {
                if (ctCancelationToken.IsCancellationRequested)
                {
                    Console.WriteLine("MyTask got cancellation request");
                    ctCancelationToken.ThrowIfCancellationRequested();
                }

                Console.WriteLine("Count is " + i);
                Thread.Sleep(500);
            }

            Console.WriteLine("MyTask finished...");
        }
    }
}