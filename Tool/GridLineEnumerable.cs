using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class GridLineEnumerable : IEnumerable<Definition.GridLine>
	{
		public GridLineEnumerable(Definition.Sudoku sudoku, Definition.LineType lineType)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;

			this.lineType = lineType;
		}

		private Definition.Sudoku sudoku;
		private Definition.LineType lineType;

		IEnumerator<Definition.GridLine> IEnumerable<Definition.GridLine>.GetEnumerator()
		{
			switch (this.lineType)
			{
				case Definition.LineType.Row:
					return TravelRow(this.sudoku).GetEnumerator();
				case Definition.LineType.Column:
					return TravelColumn(this.sudoku).GetEnumerator();
				default: throw new NotImplementedException();
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			switch (this.lineType)
			{
				case Definition.LineType.Row:
					return TravelRow(this.sudoku).GetEnumerator();
				case Definition.LineType.Column:
					return TravelColumn(this.sudoku).GetEnumerator();
				default: throw new NotImplementedException();
			}
		}

		private IEnumerable<Definition.GridLine> TravelRow(Definition.Sudoku sudoku)
		{
			foreach (var grid in sudoku.Grids)
			{
				foreach (var row in grid.Rows)
				{
					yield return row;
				}
			}
		}

		private IEnumerable<Definition.GridLine> TravelColumn(Definition.Sudoku sudoku)
		{
			foreach (var grid in sudoku.Grids)
			{
				foreach (var column in grid.Columns)
				{
					yield return column;
				}
			}
		}
	}
}
