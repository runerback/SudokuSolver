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
				using (var exceptedSeatInGridByOther3Pattern = new Pattern.ExceptedSeatInGridByOther3(sudoku))
				{
					var patterns = new Pattern.SudokuSolverPartternBase[]
					{
						oneSeatInNineParttern, 
						oneSeatInGridLinePattern, 
						anySeatInGridLinePattern,
						twoSeatsInLinePattern,
						exceptedSeatInGridByOther3Pattern
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

			
			//still not solved, use branch
			bool solvedInBranch = false;
			//foreach (var branchSolver in CreateBinaryBranch(sudoku))
			foreach (var branchSolver in new SudokuSolveBranch(sudoku))
			{
				using (branchSolver)
				{
					if (branchSolver.TrySolve())
					{
						this.sudoku.UpdateByValued(branchSolver.sudoku);
						solvedInBranch = true;
						break;
					}
				}
			}

			if (solvedInBranch)
				return true;
			
			return false;
		}

		private IEnumerable<SudokuSolver> CreateBinaryBranch(Definition.Sudoku currentSudoku)
		{
			//find one Grid with seats from 2 to 9
			var branchBasedGrid = currentSudoku.Grids
				.FirstOrDefault(item => item.Elements.SeatCount() == 2); 
			if (branchBasedGrid == null)
				yield break;

			int branchBasedGridIndex = branchBasedGrid.Index;
			int branchBasedElementIndex = branchBasedGrid.Elements
				.NotValued()
				.Select(item => item.Index)
				.First();

			var remainderValues = branchBasedGrid.Elements.Values().SudokuExcept();
			int remainderValue1, remainderValue2;
			using (var remainderValueIterator = remainderValues.GetEnumerator())
			{
				remainderValueIterator.MoveNext();
				remainderValue1 = remainderValueIterator.Current;
				remainderValueIterator.MoveNext();
				remainderValue2 = remainderValueIterator.Current;
			}

			var branch1Sudoku = currentSudoku.Copy();
			branch1Sudoku
				.Grids[branchBasedGridIndex]
				.Elements[branchBasedElementIndex]
				.SetValue(remainderValue1);

			var branch2Sudoku = currentSudoku.Copy();
			branch2Sudoku
				.Grids[branchBasedGridIndex]
				.Elements[branchBasedElementIndex]
				.SetValue(remainderValue2);

			yield return new SudokuSolver(branch1Sudoku);
			yield return new SudokuSolver(branch2Sudoku);
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
