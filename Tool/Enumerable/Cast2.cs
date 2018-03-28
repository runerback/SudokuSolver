using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static partial class SudokuSolverEnumerable
	{
		public static IEnumerable<int> Cast2(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Select(item => (int)item);
		}
    }
}
