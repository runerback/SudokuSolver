using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Observers
{
	public class GridLineUpdatedEventArgs : UpdatedEventArgs
	{
		public GridLineUpdatedEventArgs(Definition.GridLine line, int index)
			: base(index)
		{
			if (line == null)
				throw new ArgumentNullException("line");
			this.line = line;
		}

		private Definition.GridLine line;
		public Definition.GridLine Line
		{
			get { return this.line; }
		}
	}
}
