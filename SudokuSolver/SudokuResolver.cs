using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
	public class SudokuResolver
	{
		public SudokuResolver(Definition.Sudoku sudoku)
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

			this.oneSeatInNinePartern = new Parterns.OneSeatInNine(sudoku);
			this.oneSeatInGridLinePatern = new Parterns.OneSeatInGridLine(sudoku);
		}

		private Definition.Sudoku sudoku;

		private Observers.CompletionStateObserver completionState;

		#region Parterns

		private Parterns.OneSeatInNine oneSeatInNinePartern;
		private Parterns.OneSeatInGridLine oneSeatInGridLinePatern;

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
