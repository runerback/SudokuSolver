using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class GridObserver : ElementValueObserver
	{
		public GridObserver(Definition.Grid grid, SeatMode seatMode)
			: base(grid, seatMode)
		{
			this.grid = grid;
		}

		private readonly Definition.Grid grid;
		public Definition.Grid Source
		{
			get { return grid; }
		}

		protected override void onElementClusterValueChanged(object elementObj, int emptyElementsCount)
		{
			if (emptyElementsCount <= this.observingSeatsCount)
			{
				if (Updated != null)
					Updated(this, new GridUpdatedEventArgs(this.grid));
			}
		}

		public event EventHandler<GridUpdatedEventArgs> Updated;
	}
}
