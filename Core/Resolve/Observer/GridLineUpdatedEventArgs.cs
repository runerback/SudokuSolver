using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal class GridLineUpdatedEventArgs : UpdatedEventArgs
	{
		public GridLineUpdatedEventArgs(Definition.GridLine line)
			: base(line.Index)
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
