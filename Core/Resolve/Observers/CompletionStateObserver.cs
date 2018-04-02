using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class CompletionStateObserver : ObserverBase
	{
		public CompletionStateObserver(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;

			foreach (var gridObserver in sudoku.Grids
				.Select(item => new GridObserver(item, SeatMode.All)))
			{
				if (gridObserver.IsIdle)
				{
					gridObserver.Dispose();
					this.completedGridCount++;
				}
				else
				{
					gridObserver.Updated += onGridUpdated;
					this.gridObservers.Add(gridObserver);
				}
			}
		}

		private Definition.Sudoku sudoku;
		private List<ElementValueObserver> gridObservers = new List<ElementValueObserver>();

		private int completedGridCount = 0;
		public bool IsCompleted
		{
			get { return this.completedGridCount == 9; }
		}

		public int SeatsRemainder()
		{
			return this.gridObservers.Sum(
				observer => observer.ElementCluster.Elements.Count(element => !element.HasValue));
		}

		private void onGridUpdated(object sender, GridUpdatedEventArgs e)
		{
			var gridObserver = (Observers.GridObserver)sender;
			if (gridObserver.IsIdle)
			{
				completedGridCount++;
				gridObserver.Updated -= onGridUpdated;
				this.gridObservers.Remove(gridObserver);
			}
		}

		protected override void Disposing()
		{
			foreach (var gridObserver in this.gridObservers)
			{
				gridObserver.Dispose();
			}
		}
	}
}
