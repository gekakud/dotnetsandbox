using System;
using System.Linq;

namespace ParallelLINQ
{
    internal class Program
    {
        private static void Main()
        {
            var numbers = new int[100000];
            
            for (var i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i;
            }

            numbers[1000] = -5;
            numbers[14000] = -2;
            numbers[35000] = -4;
            numbers[76000] = -3;
            numbers[96000] = -1;
            numbers[55000] = -6;

            //find negative number in array and order by desc.          
            var negatives = numbers.AsParallel().AsOrdered().Where(p_n => p_n < 0);
            foreach (var negative in negatives)
            {
                Console.Write(negative + "  ");
            }
            Console.WriteLine();
            
            //same thing
            ParallelQuery<int> negs = from num in numbers
                .AsParallel()
                .AsOrdered()
                where num < 0
                select num;

            foreach (var negative in negs)
            {
                Console.Write(negative + "  ");
            }

            Console.ReadKey();
        }
    }
}
