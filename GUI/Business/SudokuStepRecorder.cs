using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Business
{
	internal class SudokuStepRecorder
	{
		public SudokuStepRecorder(Model.Sudoku targetSudoku)
		{
			if (targetSudoku == null)
				throw new ArgumentNullException("targetSudoku");
		}
	}
}
