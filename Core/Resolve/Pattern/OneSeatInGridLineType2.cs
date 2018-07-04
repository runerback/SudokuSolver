using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  SudokuSolver.Core.Pattern
{
	/// <summary>
	/// 2. only one seat in grid line, focus on current line
	/// </summary>
	internal sealed class OneSeatInGridLineType2 : SudokuSolverPartternBase
	{
		public OneSeatInGridLineType2(Definition.Sudoku sudoku)
			: base(sudoku)
		{ }

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			foreach (var gridLineObserver in new GridLineEnumerable(sudoku, Definition.LineType.Both)
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

			var grid1Completed = otherGrid1
				.GetElementsInCurrentGridLine(currentLineIndex, lineType)
				.AllHasValue();
			var grid2Completed = otherGrid2
				.GetElementsInCurrentGridLine(currentLineIndex, lineType)
				.AllHasValue();
			//only when one other grid line is completed
			if (grid1Completed == grid2Completed)
				return false;

			var targetGrid = grid1Completed ? otherGrid2 : otherGrid1;

			//if one of seats value appeared in target grid, fill to complete
			var seatValues = new LineEnumerable(this.sudoku, lineType)
				//.ElementAt(currentLineIndex) //should get sudoku line index here
				.ElementAt(getSudokuLineIndex(currentLineIndex, currentGrid.Index, lineType))
				.Elements
				.Values()
				.SudokuExcept();
			var intersectResults = targetGrid.Elements.Values()
				.Except(currentGrid.Elements.Values())
				.Intersect(seatValues);

			int value = intersectResults.SingleOrDefault(-1);
			if (value > 0)
			{
				filling(emptyElement, value);
				emptyElement.SetValue(value);
				return true;
			}
			return false;
		}

		private int getSudokuLineIndex(int gridLineIndex, int gridIndex, Definition.LineType lineType)
		{
			switch (lineType)
			{
				case Definition.LineType.Row:
					return (gridIndex / 3) * 3 + gridLineIndex;
				case Definition.LineType.Column:
					return (gridIndex % 3) * 3 + gridLineIndex;
				default: throw new InvalidOperationException(lineType.ToString());
			}
		}
		
		public override void Fill()
		{
			foreach (var gridLine in new GridLineEnumerable(sudoku, Definition.LineType.Both))
			{
				fillOnlyOneElement(gridLine);

				if (HasFailed)
					break;
			}
		}

		private const int index = 2;
		public override int Index
		{
			get { return index; }
		}
	}
}
