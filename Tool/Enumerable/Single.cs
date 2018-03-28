using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		public static Definition.Element SingleNoValueOrDefault(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.SingleOrDefault(item => !item.HasValue);
		}

		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			using (IEnumerator<TSource> e = source.GetEnumerator())
			{
				if (!e.MoveNext()) return defaultValue;
				TSource result = e.Current;
				if (!e.MoveNext()) return result;
			}
			return defaultValue;
		}
	}
}
