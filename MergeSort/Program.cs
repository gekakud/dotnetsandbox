using System;
using System.Collections.Generic;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> unsorted = new List<int> { 33, 12, 86, 8, 10, 55, 69, 1, 113 };
            MergeSort mergeSort = new MergeSort();
            var sorted = mergeSort.Sort(unsorted);
            foreach (var i in sorted)
            {
                Console.WriteLine(i);
            }

            int[] data = { 17, 20, 11, 8, 0, 1, 14, 9, 9, 15, 5, 12, 8, 11, 16, 11, 11, 9, 16, 18 };
            QuickSort.Sort(data, 0, data.Length-1);

            Console.ReadKey();
        }
    }

    class QuickSort
    {
        static int Partition(int[] array, int low, int high)
        {
            //1. Select a pivot point.
            int pivot = array[high];

            int lowIndex = (low - 1);

            //2. Reorder the collection.
            for (int j = low; j < high; j++)
            {
                if (array[j] <= pivot)
                {
                    lowIndex++;

                    int temp = array[lowIndex];
                    array[lowIndex] = array[j];
                    array[j] = temp;
                }
            }

            int temp1 = array[lowIndex + 1];
            array[lowIndex + 1] = array[high];
            array[high] = temp1;

            return lowIndex + 1;
        }

        public static void Sort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(array, low, high);

                //3. Recursively continue sorting the array
                Sort(array, low, partitionIndex - 1);
                Sort(array, partitionIndex + 1, high);
            }
        }
    }
}
