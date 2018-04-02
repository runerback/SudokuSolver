using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal sealed class TwoSeatsInLine : SudokuSolverPartternBase
	{
		public TwoSeatsInLine(Definition.Sudoku sudoku)
			: base(sudoku)
		{
		}

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			throw new NotImplementedException();
		}

		public override void Fill()
		{
			throw new NotImplementedException();
		}
	}
}
