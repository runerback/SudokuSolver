using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Core
{
	internal sealed class SudokuValidator : IDisposable
	{
		public SudokuValidator(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			this.observers = registerObservers(sudoku).ToArray();
		}

		private IEnumerable<Observers.ObserverBase> observers;

		private IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			foreach (var gridObserver in sudoku.Grids
				.Select(item => new Observers.GridObserver(item, Observers.SeatMode.All)))
			{
				if (gridObserver.IsIdle)
				{
					gridObserver.Dispose();
				}
				else
				{
					gridObserver.Updated += onElementClusterUpdated;
					yield return gridObserver;
				}
			}

			foreach (var lineObserver in new LineEnumerable(sudoku, Definition.LineType.Row)
				.Concat(new LineEnumerable(sudoku, Definition.LineType.Column))
				.Select(item => new Observers.LineObserver(item, Observers.SeatMode.All)))
			{
				if (lineObserver.IsIdle)
				{
					lineObserver.Dispose();
				}
				else
				{
					lineObserver.Updated += onElementClusterUpdated;
					yield return lineObserver;
				}
			}
		}

		private void onElementClusterUpdated(object sender, Observers.ElementClusterUpdatedEventArgs e)
		{
			if (!e.ElementCluster.Elements.Validate())
				Failed();
		}

		private void Failed()
		{
			this.hasFailed = true;
			Dispose(true);
		}

		private bool hasFailed = false;
		public bool HasFailed
		{
			get { return this.hasFailed; }
		}

		#region IDisposable

		private bool disposed;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				foreach (var observer in this.observers)
				{
					observer.Dispose();
				}
				this.observers = null;
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
