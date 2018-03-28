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
				.Select(item => item.Value.Value);
		}
	}
}
