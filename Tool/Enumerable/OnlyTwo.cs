using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		public static IEnumerable<TSource> OnlyTwo<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			using (var iterator = source.GetEnumerator())
			{
				TSource one, two;

				if (!iterator.MoveNext()) yield break;
				one = iterator.Current;

				if (!iterator.MoveNext()) yield break;
				two = iterator.Current;

				if (iterator.MoveNext()) yield break;

				yield return one;
				yield return two;
			}
		}

		public static IEnumerable<TSource> OnlyTwo<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			return source.Where(predicate).OnlyTwo();
		}
	}
}
