using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelExample
{
    internal class Program
    {
        //Using the “Parallel” class we can implement parallelism. 
        //Parallelism differs from the Threading in a way that it uses
        //all the available CPU or core. Two type of parallelism is possible:
        //Data Parallelism and Task Parallelism
        private static void Main()
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount > 1 ? Environment.ProcessorCount - 1 : Environment.ProcessorCount
            };

            // 1 - Task Parallelism: If we want to run multiple task in parallel 
            //we can use task parallelism by calling the invoke method of Parallel class. 
            //Parallel.Invoke method accepts an array of Action delegate
            Parallel.Invoke(options, MyTask, MyTask, MyTask);
            //Main Task is waiting for all methods passed to Invoke until completed


            // 2 - Data Parallelism: If we have a big collection of data and 
            //we want some operation on each of the data to perform in parallel, then we can use data parallelism.
            //Parallel class has static For or ForEach method to perform data parallelism
            ParallelLoopResult result =
                Parallel.For(0, 100, async (int i) =>
                {
                    Console.WriteLine("{0}, task: {1}, thread: {2}", i,
                        Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                    await Task.Delay(10);

                });


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
