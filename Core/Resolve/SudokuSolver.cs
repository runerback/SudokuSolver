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
			if (!sudoku.Validate())
				throw new ArgumentException("Invalid Sudoku. Duplicated element found");

			this.sudoku = sudoku;
			this.completionState = new Observers.CompletionStateObserver(sudoku);
		}

		private Definition.Sudoku sudoku;
		private Observers.CompletionStateObserver completionState;

		public bool TrySolve()
		{
			var sudoku = this.sudoku;
			var completionState = this.completionState;

			if (completionState.IsCompleted) //the sudoku might be already completed
				return true;

			int lastSeatsRemainder = completionState.SeatsRemainder();
			while (true)
			{
				using (var oneSeatInNineParttern = new Pattern.OneSeatInNine(sudoku))
				using (var oneSeatInGridLinePattern = new Pattern.OneSeatInGridLine(sudoku))
				using (var oneSearInGridLinePattern2 = new Pattern.OneSeatInGridLineType2(sudoku))
				//using (var anySeatInGridLinePattern = new Pattern.AnySeatInGridLine(sudoku))
				using (var twoSeatsInLinePattern = new Pattern.TwoSeatsInLine(sudoku))
				using (var anySeatInGridLinePattern2 = new Pattern.AnySeatInGridLineType2(sudoku))
				{
					var patterns = new Pattern.SudokuSolverPartternBase[]
					{
						oneSeatInNineParttern, 
						oneSeatInGridLinePattern, 
						oneSearInGridLinePattern2, 
						//anySeatInGridLinePattern, 
						twoSeatsInLinePattern,
						anySeatInGridLinePattern2
					};
					foreach (var pattern in patterns.OrderBy(item => item.Index))
					{
						pattern.Fill();
						if (pattern.HasFailed)
							return false;
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

			//still not solved, use branch
			Definition.Sudoku completedSudoku = null;
			if (trySolveInBranch(out completedSudoku))
			{
				sudoku.UpdateByValued(completedSudoku);
				return true;
			}
			return false;
		}

		private bool trySolveInBranch(out Definition.Sudoku completedSudoku)
		{
			foreach (var branchSolver in new Branch.SudokuSolveBranch(sudoku))
			{
				using (branchSolver)
				{
					if (branchSolver.TrySolve())
					{
						completedSudoku = branchSolver.sudoku;
						return true;
					}
				}
			}

			completedSudoku = null;
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
