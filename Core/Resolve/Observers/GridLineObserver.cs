using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class GridLineObserver : ElementValueObserver
	{
		public GridLineObserver(Definition.GridLine line, SeatMode seatMode)
			: base(line, seatMode)
		{
			this.line = line;
		}

		private Definition.GridLine line;

		protected override void onElementClusterValueChanged(object elementObj, int emptyElementsCount)
		{
			switch (this.SeatMode)
			{
				case Observers.SeatMode.One:
					{
						if (emptyElementsCount <= 1)
						{
							if (Updated != null)
								Updated(this, new GridLineUpdatedEventArgs(this.line));
						}
					}
					break;
				case Observers.SeatMode.Any:
					{
						if (Updated != null)
							Updated(this, new GridLineUpdatedEventArgs(this.line));
					}
					break;
				default: throw new NotImplementedException();
			}
		}

		public event EventHandler<GridLineUpdatedEventArgs> Updated;
	}
}
