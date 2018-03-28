using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class GridUpdatedEventArgs : UpdatedEventArgs
	{
		public GridUpdatedEventArgs(Definition.Grid grid)
			: base(grid.Index)
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
