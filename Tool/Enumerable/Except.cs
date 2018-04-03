using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		private static IEnumerable<int> SudokuElements()
		{
			return Enumerable.Range(1, 9);
		}

		public static IEnumerable<int> SudokuExcept(this IEnumerable<int> source)
		{
			if (source == null) 
				throw new ArgumentNullException("source");

			return SudokuElements().Except(source);
		}

		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> source, TSource exceptValue)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Except(Enumerable.Repeat(exceptValue, 1));
		}

		public static IEnumerable<TSource> ExceptWithoutDisdinct<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			return ExceptWithoutDisdinctIterator(first, second);
		}

		private static IEnumerable<TSource> ExceptWithoutDisdinctIterator<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			HashSet<TSource> set = new HashSet<TSource>();
			foreach (var item in second) set.Add(item);
			foreach (var item in first)
			{
				if (!set.Contains(item))
					yield return item;
			}
		}

		public static IEnumerable<TSource> ExceptWithoutDisdinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEnumerable<TKey> keys)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (keySelector == null)
				throw new ArgumentNullException("keySelector");
			return ExceptByKeyWithoutDisdinctIterator(source, keySelector, keys);
		}

		private static IEnumerable<TSource> ExceptByKeyWithoutDisdinctIterator<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEnumerable<TKey> keys)
		{
			HashSet<TKey> set = new HashSet<TKey>();
			foreach (var item in keys) set.Add(item);
			foreach (var item in source)
			{
				if (!set.Contains(keySelector(item)))
					yield return item;
			}
		}
	}
}
