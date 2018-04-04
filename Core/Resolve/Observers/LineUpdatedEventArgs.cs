using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal sealed class LineUpdatedEventArgs : ElementClusterUpdatedEventArgs
	{
		public LineUpdatedEventArgs(Definition.Line line)
			: base(line.Index, line)
		{
			this.line = line;
		}

		private Definition.Line line;
		public Definition.Line Line
		{
			get { return this.line; }
		}
	}
}
