using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		public static Definition.Element FirstHasValueOrDefault(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.FirstOrDefault(item => item.HasValue);
		}

		public static Definition.Element FirstNoValueOrDefault(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.FirstOrDefault(item => !item.HasValue);
		}
	}
}
