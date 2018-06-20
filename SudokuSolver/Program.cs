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
			try
			{
				//SudokuBuildAndResolve.BuildAndResolve(1666468307, true);
				SudokuParseAndResolve.Resolve();
				//SudokuSeedsCollector.Collect();
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp.ToString());
			}
			finally
			{
				Console.WriteLine("press any key to continue . . .");
				Console.ReadKey(true);

				SudokuParseAndResolve.Dispose();
			}
		}
	}
}
