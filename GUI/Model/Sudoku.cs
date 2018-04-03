using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Model
{
	public class Sudoku
	{
		public Sudoku()
		{
			this.grids = Enumerable.Range(0, 9)
				.Select(item => new Grid(item))
				.ToArray();
		}

		private Grid[] grids;
		public Grid[] Grids
		{
			get { return this.grids; }
		}

		internal void Sync(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			using (var gridIterator = sudoku.Grids.AsEnumerable().GetEnumerator())
			using (var thisGridIterator = this.Grids.AsEnumerable().GetEnumerator())
			{
				while (gridIterator.MoveNext() & thisGridIterator.MoveNext())
				{
					using(var elementIterator = gridIterator.Current.Elements.AsEnumerable().GetEnumerator())
					using (var thisElementIterator = thisGridIterator.Current.Elements.AsEnumerable().GetEnumerator())
					{
						while (elementIterator.MoveNext() & thisElementIterator.MoveNext())
						{
							var element = elementIterator.Current;
							var thisElement = thisElementIterator.Current;

							if (element.HasValue)
								thisElement.Value = element.Value;
							else
								thisElement.Value = null;
						}
					}
				}
			}
		}
	}
}
