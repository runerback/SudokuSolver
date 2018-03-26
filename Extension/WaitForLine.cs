using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Extension
{
	public static partial class SudokuSolverExtension
	{
		public static void WaitForLine()
		{
			while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
		}
	}
}
