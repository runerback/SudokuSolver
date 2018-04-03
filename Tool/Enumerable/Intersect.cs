using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			return IntersectCountIterator<TSource>(first, second, null);
		}

		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			return IntersectCountIterator<TSource>(first, second, comparer);
		}

		private static IEnumerable<KeyValuePair<TSource, int>> IntersectCountIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Dictionary<TSource, int> set = new Dictionary<TSource, int>(comparer);
			foreach (var item in first.Concat(second))
			{
				if (set.ContainsKey(item))
				{
					set[item]++;
				}
				else
				{
					set.Add(item, 1);
				}
			}

			foreach (var item in set)
				yield return item;
		}

		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<KeyValuePair<TSource, int>> first, IEnumerable<KeyValuePair<TSource, int>> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			return IntersectCountIterator<TSource>(first, second, null);
		}

		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<KeyValuePair<TSource, int>> first, IEnumerable<KeyValuePair<TSource, int>> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			return IntersectCountIterator<TSource>(first, second, comparer);
		}

		private static IEnumerable<KeyValuePair<TSource, int>> IntersectCountIterator<TSource>(IEnumerable<KeyValuePair<TSource, int>> first, IEnumerable<KeyValuePair<TSource, int>> second, IEqualityComparer<TSource> comparer)
		{
			Dictionary<TSource, int> set = second
				.ToDictionary(item => item.Key, item => item.Value, comparer);
			foreach (var item in first)
			{
				var key = item.Key;
				if (set.ContainsKey(key))
				{
					set[key] += item.Value;
				}
				else
				{
					set.Add(item.Key, item.Value);
				}
			}

			foreach (var item in set)
				yield return item;
		}

		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<TSource> first, IEnumerable<KeyValuePair<TSource, int>> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			return IntersectCountIterator<TSource>(first, second, null);
		}

		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<TSource> first, IEnumerable<KeyValuePair<TSource, int>> second, IEqualityComparer<TSource> comparer)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			return IntersectCountIterator<TSource>(first, second, comparer);
		}

		private static IEnumerable<KeyValuePair<TSource, int>> IntersectCountIterator<TSource>(this IEnumerable<TSource> first, IEnumerable<KeyValuePair<TSource, int>> second, IEqualityComparer<TSource> comparer)
		{
			Dictionary<TSource, int> set = second
				.ToDictionary(item => item.Key, item => item.Value, comparer);
			foreach (var item in first)
			{
				if (set.ContainsKey(item))
					set[item]++;
				else
					set.Add(item, 1);
			}

			foreach (var item in set)
				yield return item;
		}

		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<KeyValuePair<TSource, int>> first, IEnumerable<TSource> second)
		{
			return IntersectCount(second, first);
		}

		public static IEnumerable<KeyValuePair<TSource, int>> IntersectCount<TSource>(this IEnumerable<KeyValuePair<TSource, int>> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			return IntersectCount(second, first, comparer);
		}

		public static IEnumerable<KeyValuePair<TSource, int>> EmptyIntersectCount<TSource>()
		{
			yield break;
		}
	}
}
