using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal class GridObserver
	{
		public GridObserver(Definition.Grid grid, SeatMode seatMode)
		{
			if (grid == null)
				throw new ArgumentNullException("grid");

			this.grid = grid;
			this.seatMode = seatMode;

			var uncompletedElements = grid.Elements.Where(item => !item.HasValue);

			if (!uncompletedElements.Any())
			{
				this.isIdel = true;
			}
			else
			{
				foreach (var element in uncompletedElements)
				{
					element.ValueChanged += onElementValueChanged;
				}
			}
		}

		private Definition.Grid grid;

		private SeatMode seatMode;
		public SeatMode SeatMode
		{
			get { return this.seatMode; }
		}

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var grid = this.grid;

			int emptyElementCount = grid.Elements.Count(item => !item.HasValue);
			if (emptyElementCount <= 1)
			{
				if (emptyElementCount == 0)
					UntraceCompletedGrid(grid);

				if (Updated != null)
					Updated(this, new GridUpdatedEventArgs(grid));
			}
		}

		private void UntraceCompletedGrid(Definition.Grid grid)
		{
			if (grid.Elements.All(item => item.HasValue))
			{
				foreach (var element in grid.Elements)
				{
					element.ValueChanged -= onElementValueChanged;
				}
				this.isIdel = true;
			}
		}

		public event EventHandler<GridUpdatedEventArgs> Updated;

		private bool isIdel = false;
		public bool IsIdle
		{
			get { return this.isIdel; }
		}
	}
}
