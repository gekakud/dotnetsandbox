using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            //run cpu intensive with only 1 thread - we see cpu utilization is very bad
            var res = PrimesInRangeNaive(1,400000);
            Thread.Sleep(2000);

            //optimizing - split work between several threads
            //cpu utilization is better now, but not distributed equally between thread
            //some threads finish faster than others, bringing the overall CPU utilization to much lower than 100 %
            res = PrimesInRangeByChunks(1, 400000);
            Thread.Sleep(2000);

            // Thread POOL
            //From manual thread management, the natural first step was towards thread pools. A thread pool is a
            //component that manages a bunch of threads available for work item execution.Instead of creating a thread to
            //perform a certain task, you queue that task to the thread pool, which selects an available thread and dispatches
            //that task for execution.Thread pools help address some of the problems highlighted above
            //Let ThreadPool decide...
            res = PrimesInRangeOnThreadPool(1, 400000);
            Thread.Sleep(2000);

            //Tasks
            res = PrimesInRangeWithTasks(1, 400000).Result;
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        // naïve version of the code that runs on a single CPU thread
        //Returns all the prime numbers in the range [start, end)
        public static IEnumerable<uint> PrimesInRangeNaive(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            for (uint number = start; number < end; ++number)
            {
                if (IsPrime(number))
                {
                    primes.Add(number);
                }
            }

            return primes;
        }

        //creating multiple threads to plit the work
        public static IEnumerable<uint> PrimesInRangeByChunks(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            uint range = end - start;
            uint numThreads = (uint)Environment.ProcessorCount; //is this a good idea?
            uint chunk = range / numThreads; //hopefully, there is no remainder
            Thread[] threads = new Thread[numThreads];
            for (uint i = 0; i < numThreads; ++i)
            {
                uint chunkStart = start + i * chunk;
                uint chunkEnd = chunkStart + chunk;
                threads[i] = new Thread(() => {
                    for (uint number = chunkStart; number < chunkEnd; ++number)
                    {
                        if (IsPrime(number))
                        {
                            lock (primes)
                            {
                                primes.Add(number);
                            }
                        }
                    }
                });
                threads[i].Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            return primes;
        }

        //sending work chunks to ThreadPool
        public static IEnumerable<uint> PrimesInRangeOnThreadPool(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            const uint ChunkSize = 100;
            int completed = 0;
            ManualResetEvent allDone = new ManualResetEvent(initialState: false);
            uint chunks = (end - start) / ChunkSize; //again, this should divide evenly
            for (uint i = 0; i < chunks; ++i)
            {
                uint chunkStart = start + i * ChunkSize;
                uint chunkEnd = chunkStart + ChunkSize;
                ThreadPool.QueueUserWorkItem(_ => {
                    for (uint number = chunkStart; number < chunkEnd; ++number)
                    {
                        if (IsPrime(number))
                        {
                            lock (primes)
                            {
                                primes.Add(number);
                            }
                        }
                    }
                    if (Interlocked.Increment(ref completed) == chunks)
                    {
                        allDone.Set();
                    }
                });
            }
            allDone.WaitOne();
            return primes;
        }

        //creating tasks
        public static async Task<IEnumerable<uint>> PrimesInRangeWithTasks(uint start, uint end)
        {
            List<Task> tasks = new List<Task>();
            List<uint> primes = new List<uint>();
            const uint ChunkSize = 100;

            uint chunks = (end - start) / ChunkSize; //again, this should divide evenly
            for (uint i = 0; i < chunks; ++i)
            {
                uint chunkStart = start + i * ChunkSize;
                uint chunkEnd = chunkStart + ChunkSize;

                tasks.Add(Task.Run(() =>
                {
                    for (uint number = chunkStart; number < chunkEnd; ++number)
                    {
                        if (IsPrime(number))
                        {
                            lock (primes)
                            {
                                primes.Add(number);
                            }
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks).ContinueWith(task => { Console.WriteLine("all tasks completed"); });
            return primes;
        }

        private static bool IsPrime(uint number)
        {
            //This is a very inefficient O(n) algorithm, but it will do for our expository purposes
            if (number == 2) return true;
            if (number % 2 == 0) return false;
            for (uint divisor = 3; divisor < number; divisor += 2)
            {
                if (number % divisor == 0) return false;
            }

            return true;
        }
    }
}
