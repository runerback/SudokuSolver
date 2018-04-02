using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public sealed class LineEnumerable : IEnumerable<Definition.Line>
	{
		public LineEnumerable(Definition.Sudoku sudoku, Definition.LineType lineType)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;

			this.lineType = lineType;
		}

		private Definition.Sudoku sudoku;
		private Definition.LineType lineType;

		IEnumerator<Definition.Line> IEnumerable<Definition.Line>.GetEnumerator()
		{
			return Travel(this.sudoku, this.lineType).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Definition.Line>)this).GetEnumerator();
		}

		private IEnumerable<Definition.Line> Travel(Definition.Sudoku sudoku, Definition.LineType lineType)
		{
			switch (this.lineType)
			{
				case Definition.LineType.Row:
					return this.sudoku.Rows;
				case Definition.LineType.Column:
					return this.sudoku.Columns;
				default: throw new NotImplementedException();
			}
		}
	}
}
