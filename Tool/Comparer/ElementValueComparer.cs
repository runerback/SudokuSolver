using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public sealed class ElementValueComparer : IEqualityComparer<Definition.Element>
	{
		bool IEqualityComparer<Definition.Element>.Equals(Definition.Element x, Definition.Element y)
		{
			return x == null ? y == null : x.Equals(y);
		}

		int IEqualityComparer<Definition.Element>.GetHashCode(Definition.Element obj)
		{
			if (obj == null) return 0;
			if (!obj.HasValue) return 0;
			return obj.Value.GetHashCode();
		}
	}
}
