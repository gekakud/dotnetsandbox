using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfraUtils
{
    public static class EqualityUtil
    {
        public static int GetHashCodeByItems<T>(this IEnumerable<T> source)
        {
            return source.Aggregate(0, (s, x) => s ^ x.GetHashCode());
        }

        public static bool SafeEquals<T>(this T x, T y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool SafeEquals<T>(this T? x, T? y) where T: struct
        {
            if (!x.HasValue && !y.HasValue)
            {
                return true;
            }

            bool? res = x.HasValue
                ? y?.Equals(x.Value)
                : !y.HasValue;

            return res ?? false;
        }

        public static bool EqualDictionaries<T1, T2>(IDictionary<T1, T2> dic1, IDictionary<T1, T2> dic2)
        {
            if (ReferenceEquals(dic1, dic2))
            {
                return true;
            }
            if (dic1 == null || dic2 == null)
            {
                return false;
            }
            if (!dic1.Count.Equals(dic2.Count))
            {
                return false;
            }
            return dic1.All(dicPair => dic2.ContainsKey(dicPair.Key) && SafeEquals(dic2[dicPair.Key], dicPair.Value));
        }

        public static bool EqualDictionaries<T1, T2>(IDictionary<T1, List<T2>> dic1, IDictionary<T1, List<T2>> dic2)
        {
            if (ReferenceEquals(dic1, dic2))
            {
                return true;
            }
            if (dic1 == null || dic2 == null)
            {
                return false;
            }
            if (!dic1.Count.Equals(dic2.Count))
            {
                return false;
            }
            return dic1.All(dicPair => dic2.ContainsKey(dicPair.Key) && EqualLists(dic2[dicPair.Key], dicPair.Value));
        }

        public static bool EqualDictionaries<T1, T2>(IDictionary<T1, ISet<T2>> dic1, IDictionary<T1, ISet<T2>> dic2)
        {
            if (ReferenceEquals(dic1, dic2))
            {
                return true;
            }
            if (dic1 == null || dic2 == null)
            {
                return false;
            }
            if (!dic1.Count.Equals(dic2.Count))
            {
                return false;
            }
            return dic1.All(dicPair => dic2.ContainsKey(dicPair.Key) && EqualSets(dic2[dicPair.Key], dicPair.Value));
        }

        public static bool EqualReadOnlyDictionaries<T1, T2>(IReadOnlyDictionary<T1, T2> dic1, IReadOnlyDictionary<T1, T2> dic2)
        {
            if (ReferenceEquals(dic1, dic2))
            {
                return true;
            }
            if (dic1 == null || dic2 == null)
            {
                return false;
            }
            if (!dic1.Count.Equals(dic2.Count))
            {
                return false;
            }
            foreach (KeyValuePair<T1, T2> pair in dic1)
            {
                T2 dic2Val;
                if (!dic2.TryGetValue(pair.Key, out dic2Val) || !SafeEquals(pair.Value, dic2Val))
                    return false;
            }
            return true;
        }

        public static bool EqualIgnoreCase(this string str, string str2)
        {
            return string.Compare(str, str2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Checks if two collections equal, ignoring the elements order.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool EqualKeyedCollection<TKey, TValue>(KeyedCollection<TKey, TValue> x, KeyedCollection<TKey, TValue> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.Count.Equals(y.Count) && x.All(y.Contains);
        }

        public static bool EqualListsWithNullIsEmpty<T>(IList<T> list1, IList<T> list2)
        {
            if (ReferenceEquals(list1, list2))
            {
                return true;
            }
            if (list1 == null && list2.Count == 0 || list2 == null && list1.Count == 0)
            {
                return true;
            }
            if (list1 == null || list2 == null)
            {
                return false;
            }
            return list1.SequenceEqual(list2);
        }

        public static bool EqualLists<T>(this IList<T> list1, IList<T> list2)
        {
            return SafeSequenceEquals(list1, list2);
        }

        public static bool EqualSets<T>(this ISet<T> set1, ISet<T> set2)
        {
            if (ReferenceEquals(set1, set2))
            {
                return true;
            }
            if (set1 == null || set2 == null)
            {
                return false;
            }
            var symmetricDifference = new HashSet<T>(set1); // O(n)
            symmetricDifference.SymmetricExceptWith(set2); // O(n + m)
            if (symmetricDifference.Any())
            {
                return false;
            }
            return true;
        }

        public static bool EqualEnumerablesOrderAgnostic<T>(IEnumerable<T> en1, IEnumerable<T> en2)
        {
            if (ReferenceEquals(en1, en2))
            {
                return true;
            }
            if (en1 == null || en2 == null)
            {
                return false;
            }
            var symmetricDifference = new HashSet<T>(en1); // O(n)
            symmetricDifference.SymmetricExceptWith(en2); // O(n + m)
            if (symmetricDifference.Any())
            {
                return false;
            }
            return true;
        }

        public static bool SafeDateTimeEquals(DateTime time1, DateTime time2)
        {
            return time1.Year == time2.Year &&
                   time1.Month == time2.Month &&
                   time1.Day == time2.Day &&
                   time1.Hour == time2.Hour &&
                   time1.Minute == time2.Minute &&
                   time1.Second == time2.Second &&
                   time1.Kind == time2.Kind;
        }

        public static bool SafeDateTimeEquals(DateTime? time1, DateTime? time2)
        {
            if (time1 == null && time2 == null)
            {
                return true;
            }
            if (time1 == null || time2 == null)
            {
                return false;
            }
            return time1.Value.Year == time2.Value.Year &&
                   time1.Value.Month == time2.Value.Month &&
                   time1.Value.Day == time2.Value.Day &&
                   time1.Value.Hour == time2.Value.Hour &&
                   time1.Value.Minute == time2.Value.Minute &&
                   time1.Value.Second == time2.Value.Second &&
                   time1.Value.Kind == time2.Value.Kind;
        }

        public static bool SafeSequenceEquals<T>(this IEnumerable<T> leftEnumerable, IEnumerable<T> rightEnumerable)
        {
            if (ReferenceEquals(leftEnumerable, rightEnumerable))
            {
                return true;
            }
            if (leftEnumerable == null || rightEnumerable == null)
            {
                return false;
            }
            return leftEnumerable.SequenceEqual(rightEnumerable);
        }

        public static bool EqualsOrderNoMatter<T>(this IList<T> oneList, IList<T> anotherList)
        {
            if (ReferenceEquals(oneList, anotherList))
            {
                return true;
            }

            if (oneList == null || anotherList == null)
            {
                return false;
            }

            if (oneList.Count != anotherList.Count)
            {
                return false;
            }

            return EqualsOrderNoMatterImpl(oneList, anotherList);
        }

        public static bool EqualsOrderNoMatter<T>(this IEnumerable<T> oneCollection, IEnumerable<T> anotherCollection)
        {
            if (ReferenceEquals(oneCollection, anotherCollection))
            {
                return true;
            }

            if (oneCollection == null || anotherCollection == null)
            {
                return false;
            }

            return EqualsOrderNoMatterImpl(oneCollection, anotherCollection);
        }

        private static bool EqualsOrderNoMatterImpl<T>(IEnumerable<T> oneCollection, IEnumerable<T> anotherCollection)
        {
            Dictionary<T, int> occurencesCountDictionary = new Dictionary<T, int>();

            // First run just populates a dictionary with count of item occurences in first collection
            foreach (T item in oneCollection)
            {
                if (occurencesCountDictionary.ContainsKey(item))
                {
                    occurencesCountDictionary[item]++;
                }
                else
                {
                    occurencesCountDictionary[item] = 1;
                }
            }

            // Second run over a second and checks if item was found in first run
            foreach (T item in anotherCollection)
            {
                int count;

                if (!occurencesCountDictionary.TryGetValue(item, out count))
                {
                    // not found -> explicit diff
                    return false;
                }

                // second collection has more such items than first
                if (count == 0)
                {
                    return false;
                }

                occurencesCountDictionary[item]--;
            }

            return occurencesCountDictionary.Values.All(c => c == 0);
        }
    }

    public class ValueWithCondition<TClass> where TClass : class
    {
        public TClass Value { get; set; }
        public bool Condition { get; set; }
    }

    public class DescComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            return Comparer<T>.Default.Compare(y, x);
        }
    }
}