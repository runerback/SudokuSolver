using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	public sealed class GridUpdatedEventArgs : UpdatedEventArgs
	{
		public GridUpdatedEventArgs(Definition.Grid grid, int index)
			: base(index)
		{
			if (grid == null)
				throw new ArgumentNullException("grid");
			this.grid = grid;
		}

		private Definition.Grid grid;
		public Definition.Grid Grid
		{
			get { return this.grid; }
		}
	}
}
