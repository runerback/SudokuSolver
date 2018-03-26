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
		}

		protected readonly Definition.Sudoku sudoku;

		private bool showStep = false;
		public bool ShowStep
		{
			get { return this.showStep; }
			set
			{
				if (this.showStep != value)
				{
					this.showStep = value;
				}
			}
		}
	}
}
