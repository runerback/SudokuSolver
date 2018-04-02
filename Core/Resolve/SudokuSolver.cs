using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core
{
	public class SudokuSolver : IDisposable
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

		public bool TrySolve()
		{
			var sudoku = this.sudoku;
			var completionState = this.completionState;

			int lastSeatsRemainder = 81;
			while (true)
			{
				using (var oneSeatInNineParttern = new Pattern.OneSeatInNine(sudoku))
				using (var oneSeatInGridLinePattern = new Pattern.OneSeatInGridLine(sudoku))
				using (var anySeatInGridLinePattern = new Pattern.AnySeatInGridLine(sudoku))
				using (var twoSeatsInLinePattern = new Pattern.TwoSeatsInLine(sudoku))
				{
					var patterns = new Pattern.SudokuSolverPartternBase[]
					{
						oneSeatInNineParttern, 
						oneSeatInGridLinePattern, 
						anySeatInGridLinePattern,
						twoSeatsInLinePattern
					};
					foreach (var pattern in patterns.OrderBy(item => item.Index))
					{
						pattern.Fill();
						if (completionState.IsCompleted)
							return true;
					}
				}

				int currentSeatsRemainder = completionState.SeatsRemainder();
				if (currentSeatsRemainder != lastSeatsRemainder)
					lastSeatsRemainder = currentSeatsRemainder;
				else
					break;
			}

			return false;
		}

		#region IDisposable

		private bool disposed;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				this.completionState.Dispose();
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable

	}
}
