using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal sealed class OneSeatInGridLine : SudokuSolverPartternBase
	{
		public OneSeatInGridLine(Definition.Sudoku sudoku)
			: base(sudoku)
		{ }

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			foreach (var gridLineObserver in new GridLineEnumerable(sudoku, Definition.LineType.Row)
				.Concat(new GridLineEnumerable(sudoku, Definition.LineType.Column))
				.Select(item => new Observers.GridLineObserver(item, Observers.SeatMode.One)))
			{
				if (gridLineObserver.IsIdle)
				{
					gridLineObserver.Dispose();
				}
				else
				{
					gridLineObserver.Updated += onGridLineUpdated;
					yield return gridLineObserver;
				}
			}
		}

		private void onGridLineUpdated(object sender, Observers.GridLineUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Line))
			{
				if (e.Line.Elements.AllHasValue())
					((Observers.GridLineObserver)sender).Updated -= onGridLineUpdated;
			}
		}

		private bool fillOnlyOneElement(Definition.GridLine gridLine)
		{
			//get empty element
			Definition.Element emptyElement = null;
			using (var e = gridLine.Elements
				.Where(item => !item.HasValue)
				.GetEnumerator())
			{
				if (!e.MoveNext()) return true; //not empty element
				emptyElement = e.Current;
				if (e.MoveNext()) return false; //more than one empty element
			}

			var lineType = gridLine.LineType;

			//get two other grids
			var currentGrid = gridLine.Grid;

			Definition.Grid otherGrid1, otherGrid2;
			currentGrid.GetOtherGrids(lineType, out otherGrid1, out otherGrid2);

			int currentLineIndex = gridLine.Index;

			var otherElementValues1 = otherGrid1.GetElementsInOtherGridLine(currentLineIndex, lineType)
				.Values();

			var otherElementValues2 = otherGrid2.GetElementsInOtherGridLine(currentLineIndex, lineType)
				.Values();

			var currentElementValues = currentGrid.Elements
				.Values();

			var exceptResult = otherElementValues1
				.Concat(otherElementValues2)
				.Except(currentElementValues);

			if (!exceptResult.Any())
				return false; //no result

			//get value which appeared both
			var bothOtherElementValues = otherElementValues1.Intersect(otherElementValues2);
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
			IEnumerable<Definition.GridLine> gridLines = new GridLineEnumerable(sudoku, Definition.LineType.Row)
				.Concat(new GridLineEnumerable(sudoku, Definition.LineType.Column));

			foreach (var gridLine in gridLines)
			{
				fillOnlyOneElement(gridLine);
			}
		}

		private const int index = 1;
		public override int Index
		{
			get { return index; }
		}
	}
}
