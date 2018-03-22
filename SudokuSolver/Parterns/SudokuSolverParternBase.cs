using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Parterns
{
	public class SudokuSolverParternBase
	{
		public SudokuSolverParternBase(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;
			this.blockEnumerable = new SudokuBlockEnumerable(sudoku);
		}

		protected readonly Definition.Sudoku sudoku;
		protected readonly SudokuBlockEnumerable blockEnumerable;

	}
}
