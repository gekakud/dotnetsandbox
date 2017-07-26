using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelLINQ
{
    internal class Program
    {
        private static void Main()
        {
            var numbers = new int[1000000];
            
            for (var i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i;
            }

            numbers[1000] = -5;
            numbers[14000] = -2;
            numbers[15000] = -2;
            numbers[16000] = -2;
            numbers[35000] = -4;
            numbers[76000] = -3;
            numbers[96000] = -1;
            numbers[55000] = -6;
            numbers[555000] = -6;
            numbers[755000] = -6;
            numbers[955000] = -6;

            //find negative number in array using ParallelQuery ?
            var watch = Stopwatch.StartNew();
            ParallelQuery<int> negatives = numbers.AsParallel().Where(p_n => p_n < 0);
            //OR
//            ParallelQuery<int> negs = from num in numbers
//                .AsParallel()
//                .AsOrdered()
//                                        where num < 0
//                                        select num;
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);
            watch.Reset();
            ShowResult(negatives);
            
            //same thing but using Parallel for each loop
            //this runs slower because of ConcurrentBag "locking"(no actual lock on concurrent collections)
            var bag = new ConcurrentBag<int>();
            watch.Start();

            Parallel.ForEach(numbers, num =>
            {
                if (num < 0)
                {
                    bag.Add(num);
                }
            });

            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);
            watch.Reset();
            ShowResult(bag.ToArray());


            //same thing but using sequencial simple loop
            //this will be the fast, because we do not make any complex calculations
            //and do not waste time for creating Tasks
            var list = new List<int>();
            watch.Start();

            foreach (var number in numbers)
            {
                if (number < 0)
                {
                    list.Add(number);
                }
            }

            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);

            ShowResult(list.ToArray());
            Console.ReadKey();
        }

        private static void ShowResult(IEnumerable<int> p_negatives)
        {
            foreach (var negative in p_negatives)
            {
                Console.Write(negative + "  ");
            }
            Console.WriteLine();
        }
    }
}
