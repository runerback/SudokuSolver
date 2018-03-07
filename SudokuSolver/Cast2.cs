using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Linq
{
	public static partial class Enumerable
	{
		public static IEnumerable<TResult> Cast2<TSource, TResult>(this IEnumerable<TSource> source)
		{
			//return source.Select(item => (TResult)item);
			throw new InvalidOperationException("this cannot happen in generic world");
		}
	}
}
