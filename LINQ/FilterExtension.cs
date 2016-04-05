using System.Collections.Generic;

namespace LINQ
{
    public static class FilterExtension
    {
        public static IEnumerable<string> StringThatStartWith(this IEnumerable<string> input, string start)
        {
            foreach (var curr in input)
            {
                if (curr.StartsWith(start))
                {
                    yield return curr;
                }
            }
        }

        public delegate bool FilterDelegate<T>(T item);

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> input, FilterDelegate<T> predicate)
        {
            foreach (var curr in input)
            {
                if (predicate(curr))
                {
                    yield return curr;
                }
            }
        } 
    }
}