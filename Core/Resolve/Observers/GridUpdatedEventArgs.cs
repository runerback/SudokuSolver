using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class GridUpdatedEventArgs : ElementClusterUpdatedEventArgs
	{
		public GridUpdatedEventArgs(Definition.Grid grid)
			: base(grid.Index, grid)
		{
			this.grid = grid;
		}

		private Definition.Grid grid;
		public Definition.Grid Grid
		{
			get { return this.grid; }
		}
	}
}
