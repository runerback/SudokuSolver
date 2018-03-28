using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core
{
	public class SudokuSolver
	{
		public SudokuSolver(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;

			if (!new SudokuValidator().Valdiate(sudoku))
			{
				Console.WriteLine("Invalid Sudoku. Duplicated element found");
				return;
			}

			this.completionState = new Observers.CompletionStateObserver(sudoku);

			this.oneSeatInNinePartern = new Pattern.OneSeatInNine(sudoku);
			this.oneSeatInGridLinePatern = new Pattern.OneSeatInGridLine(sudoku);
		}

		private Definition.Sudoku sudoku;

		private Observers.CompletionStateObserver completionState;

		#region Parterns

		private Pattern.OneSeatInNine oneSeatInNinePartern;
		private Pattern.OneSeatInGridLine oneSeatInGridLinePatern;

		#endregion Parterns

		public bool TryResolve()
		{
			this.oneSeatInNinePartern.Fill();

			if (this.completionState.IsCompleted)
				return true;

			this.oneSeatInGridLinePatern.Fill();
			if (this.completionState.IsCompleted)
				return true;

			return false;
		}
	}
}
