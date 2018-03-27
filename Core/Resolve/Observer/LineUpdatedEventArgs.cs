using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	public sealed class LineUpdatedEventArgs: UpdatedEventArgs
	{
		public LineUpdatedEventArgs(Definition.Line line, int index)
			: base(index)
		{
			if (line == null)
				throw new ArgumentNullException("line");
			this.line = line;
		}

		private Definition.Line line;
		public Definition.Line Line
		{
			get { return this.line; }
		}
	}
}
