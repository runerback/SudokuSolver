using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal class CompletionStateObserver
	{
		public CompletionStateObserver(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;

			foreach (var gridObserver in sudoku.Grids
				.Select(item => new GridObserver(item, SeatMode.Any)))
			{
				if (gridObserver.IsIdle)
				{
					this.completedGridCount++;
				}
				else
				{
					gridObserver.Updated += onGridUpdated;
				}
			}
		}

		private Definition.Sudoku sudoku;

		private int completedGridCount = 0;
		public bool IsCompleted
		{
			get { return this.completedGridCount == 9; }
		}

		private bool IsGridCompleted(Definition.Grid grid)
		{
			return grid.Elements.All(item => item.HasValue);
		}

		private void onGridUpdated(object sender, GridUpdatedEventArgs e)
		{
			var gridObserver = (Observers.GridObserver)sender;
			if (gridObserver.IsIdle)
			{
				completedGridCount++;
				gridObserver.Updated -= onGridUpdated;
			}
		}
	}
}
