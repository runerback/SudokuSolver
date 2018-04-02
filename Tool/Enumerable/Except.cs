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
	}
}
