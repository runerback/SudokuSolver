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
			if (x == null) return y == null;
			if (y == null) return false;
			if (x.HasValue != y.HasValue) return false;
			if (!x.HasValue) return true;
			return x.Value == y.Value;
		}

		int IEqualityComparer<Definition.Element>.GetHashCode(Definition.Element obj)
		{
			if (obj == null) return 0;
			if (!obj.HasValue) return 0;
			return obj.Value.GetHashCode();
		}
	}
}
