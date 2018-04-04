using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public sealed class SudokuElementEnumerable : IEnumerable<Definition.Element>
	{
		public SudokuElementEnumerable(Definition.Sudoku sudoku, SudokuElementTravelType travelType)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;
			this.travelType = travelType;
		}

		public SudokuElementEnumerable(Definition.Sudoku sudoku)
			: this(sudoku, SudokuElementTravelType.Grid)
		{
		}

		private Definition.Sudoku sudoku;
		private SudokuElementTravelType travelType;

		private IEnumerable<Definition.Element> Iterator(Definition.Sudoku sudoku, SudokuElementTravelType travelType)
		{
			IEnumerable<Definition.IElementCluster> clusters;
			switch (travelType)
			{
				case SudokuElementTravelType.Grid:
					clusters = sudoku.Grids;
					break;
				case SudokuElementTravelType.Row:
					clusters = sudoku.Rows;
					break;
				case SudokuElementTravelType.Column:
					clusters = sudoku.Columns;
					break;
				default: throw new NotImplementedException();
			}

			foreach (var cluster in clusters)
				foreach (var element in cluster.Elements)
					yield return element;
		}

		public IEnumerator<Definition.Element> GetEnumerator()
		{
			return Iterator(this.sudoku, this.travelType).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	public enum SudokuElementTravelType
	{
		Grid,
		Row,
		Column
	}
}
