using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class SudokuBlockEnumerable : IEnumerable<IEnumerable<Definition.Element>>
	{
		public SudokuBlockEnumerable(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;
		}

		private Definition.Sudoku sudoku;

		private IEnumerable<IEnumerable<Definition.Element>> travelBlocks(Definition.Sudoku sudoku)
		{
			foreach (var grid in sudoku.Grids)
			{
				yield return grid.Elements;
			}
			foreach (var row in sudoku.Rows)
			{
				yield return row.Elements;
			}
			foreach (var column in sudoku.Columns)
			{
				yield return column.Elements;
			}
		}

		IEnumerator<IEnumerable<Definition.Element>> IEnumerable<IEnumerable<Definition.Element>>.GetEnumerator()
		{
			return travelBlocks(this.sudoku).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return travelBlocks(this.sudoku).GetEnumerator();
		}
	}
}
