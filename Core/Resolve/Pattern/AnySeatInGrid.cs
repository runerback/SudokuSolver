using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal sealed class AnySeatInGrid : SudokuSolverPartternBase
	{
		public AnySeatInGrid(Definition.Sudoku sudoku)
			: base(sudoku)
		{ }

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			var observers = new GridEnumerable(sudoku)
				.Select(item => new Observers.GridObserver(item, Observers.SeatMode.Any))
				.Where(item => !item.IsIdle)
				.ToArray();

			foreach (var gridLineObserver in observers)
			{
				gridLineObserver.Updated += onGridUpdated;
			}

			return observers;
		}

		private void onGridUpdated(object sender, Observers.GridUpdatedEventArgs e)
		{
			if (fillAnyOneElement(e.Grid))
			{
				if (e.Grid.Elements.All(item => item.HasValue))
					((Observers.GridObserver)sender).Updated -= onGridUpdated;
			}
		}

		private bool fillAnyOneElement(Definition.Grid grid)
		{
			//get any empty element in grid
			Definition.Element emptyElement = grid.Elements.FirstOrDefault(item => !item.HasValue);
			if (emptyElement == null)
				return true;

			//get two other grids in same row
			Definition.Grid otherGridInRow1, otherGridInRow2;
			grid.GetOtherGrids(Definition.LineType.Row, out otherGridInRow1, out otherGridInRow2);

			//get two other grids in same column
			Definition.Grid otherGridInColumn1, otherGridInColumn2;
			grid.GetOtherGrids(Definition.LineType.Column, out otherGridInColumn1, out otherGridInColumn2);

			int elementIndex = emptyElement.Index;
			int rowLineIndex = elementIndex / 3;
			int columnLineIndex = elementIndex % 3;

			var otherGridInRow1Values = otherGridInRow1
				.GetElementsInOtherGridLine(rowLineIndex, Definition.LineType.Row)
				.Values();
			var otherGridInRow2Values = otherGridInRow2
				.GetElementsInOtherGridLine(rowLineIndex, Definition.LineType.Row)
				.Values();
			var otherGridInColumn1Values = otherGridInColumn1
				.GetElementsInOtherGridLine(columnLineIndex, Definition.LineType.Column)
				.Values();
			var otherGridInColumn2Values = otherGridInColumn2
				.GetElementsInOtherGridLine(columnLineIndex, Definition.LineType.Column)
				.Values();

			var currentElementValues = grid.Elements
				.Values();

			var exceptResult = otherGridInRow1Values
				.Concat(otherGridInRow2Values)
				.Concat(otherGridInColumn1Values)
				.Concat(otherGridInColumn2Values)
				.Except(currentElementValues);

			if (!exceptResult.Any())
				return false; //no result

			//get value which appeared both
			var bothOtherElementValues = otherGridInRow1Values
				.Intersect(otherGridInRow2Values)
				.Intersect(otherGridInColumn1Values)
				.Intersect(otherGridInColumn2Values);
			var exactIntersectElementValues = bothOtherElementValues.Intersect(exceptResult);

			int value = exactIntersectElementValues.SingleOrDefault(-1);
			if (value > 0)
			{
				emptyElement.SetValue(value);
				return true;
			}
			return false;
		}

		public override void Fill()
		{
			foreach (var grid in new GridEnumerable(sudoku))
			{
				fillAnyOneElement(grid);
			}
		}
	}
}
