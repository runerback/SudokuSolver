using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Extension
{
	public static partial class SudokuSolverExtension
	{
		public static IEnumerable<TSource> Concat2<TSource>(this IEnumerable<TSource> source, TSource next)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Concat(Enumerable.Repeat(next, 1));
		}
	}
}
