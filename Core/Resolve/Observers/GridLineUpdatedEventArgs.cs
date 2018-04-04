using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal class GridLineUpdatedEventArgs : ElementClusterUpdatedEventArgs
	{
		public GridLineUpdatedEventArgs(Definition.GridLine line)
			: base(line.Index, line)
		{
			this.line = line;
		}

		private Definition.GridLine line;
		public Definition.GridLine Line
		{
			get { return this.line; }
		}
	}
}
