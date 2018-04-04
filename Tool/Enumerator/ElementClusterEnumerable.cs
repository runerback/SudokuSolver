using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public sealed class ElementClusterEnumerable : IEnumerable<Definition.IElementCluster>
	{
		public ElementClusterEnumerable(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;
		}

		private Definition.Sudoku sudoku;

		private IEnumerable<Definition.IElementCluster> Iterator(Definition.Sudoku sudoku)
		{
			foreach (var grid in sudoku.Grids)
			{
				yield return grid;
			}
			foreach (var row in sudoku.Rows)
			{
				yield return row;
			}
			foreach (var column in sudoku.Columns)
			{
				yield return column;
			}
		}

		public IEnumerator<Definition.IElementCluster> GetEnumerator()
		{
			return Iterator(this.sudoku).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
