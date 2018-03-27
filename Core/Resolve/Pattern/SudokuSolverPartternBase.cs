using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal abstract class SudokuSolverPartternBase
	{
		public SudokuSolverPartternBase(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;
		}

		protected readonly Definition.Sudoku sudoku;

		public abstract void Fill();
	}
}
