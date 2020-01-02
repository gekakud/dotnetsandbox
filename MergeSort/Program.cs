﻿using System;
using System.Collections.Generic;
using System.Linq;

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

            Console.ReadKey();
        }
    }

    public class MergeSort
    {
        public List<int> Sort(List<int> unsortedArray)
        {

            if (unsortedArray.Count == 1)
            {
                return unsortedArray;
            }

            int mid = unsortedArray.Count / 2;
            var left = unsortedArray.GetRange(0, mid);
            var right = unsortedArray.GetRange(mid, unsortedArray.Count - mid);

            left = Sort(left);
            right = Sort(right);

            return MergeLeftAndRight(left, right);
        }

        public List<int> MergeLeftAndRight(List<int> left, List<int> right)
        {
            var merged = new List<int>();

            
            while (left.Any() || right.Any())
            {
                //there are elements in both
                if (left.Any() && right.Any())
                {
                    if (left.First() <= right.First())
                    {
                        merged.Add(left.First());
                        left.RemoveAt(0);
                    }
                    else
                    {
                        merged.Add(right.First());
                        right.RemoveAt(0);
                    }
                }
                else
                {
                    if (left.Any())
                    {
                        merged.AddRange(left);
                        left.Clear();
                    }

                    if (right.Any())
                    {
                        merged.AddRange(right);
                        right.Clear();
                    }
                }
            }

            return merged;
        }
    }

}
