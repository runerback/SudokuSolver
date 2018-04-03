using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		public static IEnumerable<int> Values(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source
				.Where(item => item.HasValue)
				.Select(item => item.Value);
		}

		public static IEnumerable<Definition.Element> Valued(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Where(item => item.HasValue);
		}

		public static IEnumerable<Definition.Element> NotValued(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Where(item => !item.HasValue);
		}
	}
}
