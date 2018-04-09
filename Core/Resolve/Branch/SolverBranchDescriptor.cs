using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Resolve.Branch
{
	internal sealed class SolverBranchDescriptor
	{
		private SudokuSnapshot snapshot;
		public SudokuSnapshot Snapshot
		{
			get { return this.snapshot; }
		}


	}
}
