using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver
{
	class Program
	{
		static void Main(string[] args)
		{
			SudokuBuildAndResolve.BuildAndResolve(1666468307, true);
			//SudokuSeedsCollector.Collect();
		}
	}
}
