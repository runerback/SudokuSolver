using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Extension
{
	public static partial class SudokuSolverExtension
	{
		public static IEnumerable<int> Cast2(this IEnumerable<Definition.Element> source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Select(item => (int)item);
		}
    }
}
