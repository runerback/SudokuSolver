using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public sealed class SudokuElementEnumerable : IEnumerable<Definition.Element>
	{
		public SudokuElementEnumerable(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;
		}

		private Definition.Sudoku sudoku;

		private IEnumerable<Definition.Element> Iterator(Definition.Sudoku sudoku)
		{
			foreach (var grid in sudoku.Grids)
				foreach (var element in grid.Elements)
					yield return element;
		}

		public IEnumerator<Definition.Element> GetEnumerator()
		{
			return Iterator(this.sudoku).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
