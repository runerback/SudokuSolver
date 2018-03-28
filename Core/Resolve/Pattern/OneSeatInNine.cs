using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	/// <summary>
	/// only one element not filled, in Grid or Line
	/// </summary>
	internal sealed class OneSeatInNine : SudokuSolverPartternBase
	{
		public OneSeatInNine(Definition.Sudoku sudoku)
			: base(sudoku)
		{
			this.blockEnumerable = new SudokuBlockEnumerable(sudoku);

			registerObservers(sudoku);
		}

		private readonly SudokuBlockEnumerable blockEnumerable;

		#region Block observers

		private void registerObservers(Definition.Sudoku sudoku)
		{
			foreach (var gridObserver in sudoku.Grids
				.Select(item => new Observers.GridObserver(item, Observers.SeatMode.One))
				.Where(item => !item.IsIdle))
			{
				gridObserver.Updated += onGridUpdated;
			}
			foreach (var rowObserver in sudoku.Rows
				.Select(item => new Observers.OneSeatLineObserver(item, Observers.SeatMode.One))
				.Where(item => !item.IsIdle)
				.Concat(sudoku
				.Columns.Select(item => new Observers.OneSeatLineObserver(item, Observers.SeatMode.One))
				.Where(item => !item.IsIdle)))
			{
				rowObserver.Updated += onLineUpdated;
			}
		}

		private void onGridUpdated(object sender, Observers.GridUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Grid.Elements))
				((Observers.GridObserver)sender).Updated -= onGridUpdated;
		}

		private void onLineUpdated(object sender, Observers.LineUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Line.Elements))
				((Observers.OneSeatLineObserver)sender).Updated -= onLineUpdated;
		}

		#endregion Block observers

		#region Fill

		public override void Fill()
		{
			foreach (var elements in blockEnumerable)
			{
				fillOnlyOneElement(elements);
			}
		}

		private bool fillOnlyOneElement(IEnumerable<Definition.Element> elements)
		{
			if (elements == null)
				throw new ArgumentNullException("elements");

			Definition.Element emptyElement = null;
			foreach (var element in elements
				.Where(item => !item.HasValue))
			{
				if (emptyElement == null)
					emptyElement = element;
				else
					return false; //more than one Empty element found
			}

			if (emptyElement != null)
			{
				int value = 1;
				foreach (int element in elements
					.Where(item => item.HasValue)
					.OrderBy(item => item))
				{
					if (element != value)
						break;
					value++;
				}
				emptyElement.SetValue(value);
			}

			return true;
		}

		#endregion Fill

		
	}
}
