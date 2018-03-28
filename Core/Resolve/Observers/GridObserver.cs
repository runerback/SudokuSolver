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

		private Definition.Grid grid;

		protected override void onElementClusterValueChanged(object elementObj, int emptyElementsCount)
		{
			switch (this.SeatMode)
			{
				case Observers.SeatMode.One:
					{
						if (emptyElementsCount <= 1)
						{
							if (Updated != null)
								Updated(this, new GridUpdatedEventArgs(this.grid));
						}
					}
					break;
				case Observers.SeatMode.Any:
					{
						if (Updated != null)
							Updated(this, new GridUpdatedEventArgs(this.grid));
					}
					break;
				default: throw new NotImplementedException();
			}
		}

		public event EventHandler<GridUpdatedEventArgs> Updated;
	}
}
