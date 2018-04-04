using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI
{
	class Program
	{
		[STAThread]
		static void Main()
		{
			var sudoku = new Definition.Sudoku();
			var controller = new SudokuPlayerController(sudoku, sudoku);
			controller.Show();
		}
	}
}
