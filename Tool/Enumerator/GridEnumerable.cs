using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public sealed class GridEnumerable : IEnumerable<Definition.Grid>
	{
		public GridEnumerable(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;
		}

		private Definition.Sudoku sudoku;

		public IEnumerator<Definition.Grid> GetEnumerator()
		{
			return this.sudoku.Grids.AsEnumerable().GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
