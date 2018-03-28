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
		}

		private Definition.Sudoku sudoku;

		private Observers.CompletionStateObserver completionState;

		public bool TryResolve()
		{
			var sudoku = this.sudoku;

			using (var oneSeatInNineParttern = new Pattern.OneSeatInNine(sudoku))
			{
				oneSeatInNineParttern.Fill();
				if (this.completionState.IsCompleted)
					return true;

				using (var oneSeatInGridLinePattern = new Pattern.OneSeatInGridLine(sudoku))
				{
					oneSeatInGridLinePattern.Fill();
					if (this.completionState.IsCompleted)
						return true;

					using (var anySeatInGridPattern = new Pattern.AnySeatInGrid(sudoku))
					{
						anySeatInGridPattern.Fill();
						if (this.completionState.IsCompleted)
							return true;
					}
				}
			}

			return false;
		}
	}
}
