using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndTaskTutorial
{
    class DeadlockExample
    {
        public void SimulateDeadlock()
        {
            var lock1 = new object();
            var lock2 = new object();

            Task t1 = Task.Run(() =>
            {
                lock (lock1)
                {
                    Console.WriteLine("lock1 by t1");
                    Thread.Sleep(500);
                    
                    lock (lock2)
                    {
                        Console.WriteLine("t1 locked lock2");
                    }
                }
            });

            Task t2 = Task.Run(() =>
            {
                lock (lock2)
                {
                    Thread.Sleep(500);
                    Console.WriteLine("lock2 by t2");
                    lock (lock1)
                    {
                        Console.WriteLine("t2 locked lock1");
                    }
                }
            });

            Task.WaitAll(t1, t2);
            Console.WriteLine("Finished...");
        }
    }
}
