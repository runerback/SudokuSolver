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

		private readonly Definition.GridLine line;
		public Definition.GridLine Source
		{
			get { return line; }
		}

		protected override void onElementClusterValueChanged(object elementObj, int emptyElementsCount)
		{
			if (emptyElementsCount <= this.observingSeatsCount)
			{
				if (Updated != null)
					Updated(this, new GridLineUpdatedEventArgs(this.line));
			}
		}

		public event EventHandler<GridLineUpdatedEventArgs> Updated;

		protected override void Disposing()
		{
			base.Disposing();

			if (this.Updated != null)
			{
				foreach (EventHandler<GridLineUpdatedEventArgs> handler in this.Updated.GetInvocationList())
				{
					this.Updated -= handler;
				}
			}
		}
	}
}
