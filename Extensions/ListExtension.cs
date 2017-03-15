using System;
using System.Collections.Generic;
using System.Linq;

namespace Tmc.Util
{
    public static class ListExtension
    {
        public static List<T> FetchTopItems<T>(this List<T> list, int n)
        {
            if (list == null || list.Count == 0) return new List<T>();

            int count = list.Count < n ? list.Count : n;

            List<T> result = list.Take(count).ToList();
            list.RemoveRange(0, count);
            return result;
        }

        public static bool AnyDuplicate<T>(this IEnumerable<T> source)
        {
            return AnyDuplicate(source, EqualityComparer<T>.Default);
        }

        public static bool AnyDuplicate<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (comparer == null) throw new ArgumentNullException("comparer");

            HashSet<T> set = new HashSet<T>(comparer);
            foreach (var item in source)
            {
                if (!set.Add(item)) return true;
            }
            return false;
        }

        public static bool AnyDuplicate<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            HashSet<TKey> seenKeys = new HashSet<TKey>();
            if (source is ISet<TSource>) return false;

            foreach (TSource item in source)
            {
                if (!seenKeys.Add(keySelector(item))) return true;
            }
            return false;
        }
    }
}
