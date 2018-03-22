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

			this.oneSeatInNineParttern = new Parterns.OneSeatInNine(sudoku);
		}

		private Definition.Sudoku sudoku;

		private Observers.CompletionStateObserver completionState;

		#region Parterns

		private Parterns.OneSeatInNine oneSeatInNineParttern;
		private Parterns.OneSeatInGridLine oneSeatInGridLine = null;

		#endregion Parterns

		public bool TryResolve()
		{
			this.oneSeatInNineParttern.Fill();

			if (this.completionState.IsCompleted)
				return true;

			return false;
		}
	}
}
