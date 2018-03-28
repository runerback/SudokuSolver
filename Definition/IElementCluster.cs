using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Definition
{
	public interface IElementCluster
	{
		IEnumerable<Element> Elements { get; }
	}
}
