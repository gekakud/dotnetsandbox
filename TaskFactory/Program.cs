using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskFactory
{
    internal class Program
    {
        static void Main()
        {
            Action action = () =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(200);
                    Console.WriteLine("Count is " + i + " in task" + Task.CurrentId);
                }

                Console.WriteLine("Task Finished");
            };

            var t1 = Task.Run(action);
            var t2 = Task.Run(action);

            //t1.Wait();
            //OR
            Console.WriteLine("Main waits for all tasks to finish");
            Task.WaitAll(t1,t2);

            Task<string> taskWithResult = Task<string>.Factory.StartNew(BuildText, Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine(taskWithResult.Result);
            Console.ReadKey();
        }

        private static string BuildText(object p_mainTaskId)
        {
            var str = string.Format("BuildText task returned string result: Main task with id {0} can proceed", (int)p_mainTaskId);
            return str;
        }
    }
}
