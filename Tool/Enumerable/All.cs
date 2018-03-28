using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		public static bool AllHasValue(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.All(item => item.HasValue);
		}

		public static bool AllNoValue(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.All(item => !item.HasValue);
		}
	}
}
