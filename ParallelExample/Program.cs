using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelExample
{
    internal class Program
    {
        private static void Main()
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : Environment.ProcessorCount
            };

            Parallel.Invoke(options, MyTask, MyTask, MyTask);

            //Main Task is waiting all methods passed to Invoke are completed
            Console.WriteLine("Main Task finished...");
            Console.ReadKey();
        }

        private static void MyTask()
        {
            var myID = Task.CurrentId;
            Console.WriteLine("Task " + myID + " started");
            for (var i = 0; i < 5; i++)
            {
                Console.WriteLine("Task " + myID + ": counter=" + i);
                Thread.Sleep(500);
            }

            Console.WriteLine("Task " + myID + " finished");
        }
    }
}
