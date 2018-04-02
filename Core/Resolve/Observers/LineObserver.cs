using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class LineObserver : ElementValueObserver
	{
		public LineObserver(Definition.Line line, SeatMode seatMode)
			: base(line, seatMode)
		{
			this.line = line;
		}

		private readonly Definition.Line line;
		public Definition.Line Source
		{
			get { return line; }
		}

		protected override void onElementClusterValueChanged(object elementObj, int emptyElementsCount)
		{
			if (emptyElementsCount <= this.observingSeatsCount)
			{
				if (Updated != null)
					Updated(this, new LineUpdatedEventArgs(this.line));
			}
		}

		public event EventHandler<LineUpdatedEventArgs> Updated;
	}
}
