using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class OneSeatLineObserver : ElementValueObserver
	{
		public OneSeatLineObserver(Definition.Line line, SeatMode seatMode)
			: base(line, seatMode)
		{
			this.line = line;
		}

		private Definition.Line line;

		protected override void onElementClusterValueChanged(object elementObj, int emptyElementsCount)
		{
			switch (this.SeatMode)
			{
				case Observers.SeatMode.One:
					{
						if (emptyElementsCount <= 1)
						{
							if (Updated != null)
								Updated(this, new LineUpdatedEventArgs(this.line));
						}
					}
					break;
				case Observers.SeatMode.Any:
					{
						if (Updated != null)
							Updated(this, new LineUpdatedEventArgs(this.line));
					}
					break;
				default: throw new NotImplementedException();
			}
		}

		public event EventHandler<LineUpdatedEventArgs> Updated;
	}
}
