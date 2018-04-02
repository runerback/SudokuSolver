using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal sealed class AnySeatInGridLine : SudokuSolverPartternBase
	{
		public AnySeatInGridLine(Definition.Sudoku sudoku)
			: base(sudoku)
		{ }

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			var observers = new GridEnumerable(sudoku)
				.Select(item => new Observers.GridObserver(item, Observers.SeatMode.All));

			foreach (var gridLineObserver in observers)
			{
				if (gridLineObserver.IsIdle)
				{
					gridLineObserver.Dispose();
				}
				else
				{
					gridLineObserver.Updated += onGridUpdated;
					yield return gridLineObserver;
				}
			}
		}

		private void onGridUpdated(object sender, Observers.GridUpdatedEventArgs e)
		{
			if (fillAnyOneElement(e.Grid))
			{
				if (e.Grid.Elements.All(item => item.HasValue))
					((Observers.GridObserver)sender).Updated -= onGridUpdated;
			}
		}

		private NextEmptyElementInGridHelper emptyElementHelper = new NextEmptyElementInGridHelper();

		private bool fillAnyOneElement(Definition.Grid grid)
		{
			//get any empty element in grid
			//this should be: get next empty element in grid
			Definition.Element emptyElement;// = grid.Elements.FirstOrDefault(item => !item.HasValue);
			//if (emptyElement == null)
			if (!emptyElementHelper.NextEmptyElement(grid, out emptyElement)) //got problem here
				return false;

			return fillAnyEmptyElement(grid, emptyElement);
		}

		private bool fillAnyEmptyElement(Definition.Grid grid, Definition.Element emptyElement)
		{
			if (emptyElement.HasValue)
				return false;

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
			var emptyElementHelper = this.emptyElementHelper;
			var enumerable = new GridEnumerable(sudoku);
			bool notFilledHappend = false;
			while (true)
			{
				bool filled = false;
				foreach (var grid in enumerable)
				{
					Definition.Element emptyElement;
					if (emptyElementHelper.NextEmptyElement(grid, out emptyElement))
					{
						if (fillAnyEmptyElement(grid, emptyElement) && !filled)
							filled = true;
					}
				}

				if (!filled)
				{
					if (!notFilledHappend)
						notFilledHappend = true;
					else
						break;
				}
				else
				{
					this.emptyElementHelper.ResetAllFinished();
				}
			}
		}

		private class NextEmptyElementInGridHelper
		{
			private readonly object mapLock = new object();

			private readonly Dictionary<Definition.Grid, int> gridElementMap = 
				new Dictionary<Definition.Grid, int>();

			public bool NextEmptyElement(Definition.Grid grid, out Definition.Element element)
			{
				if (grid == null)
					throw new ArgumentNullException("grid");

				element = null;

				int nextIndex = 0;
				lock (this.mapLock)
				{
					var gridElementMap = this.gridElementMap;
					if (!gridElementMap.TryGetValue(grid, out nextIndex))
					{
						for (int i = 0; i < 9; i++)
						{
							var item = grid.Elements[i];
							if (!item.HasValue)
							{
								gridElementMap.Add(grid, i + 1);
								element = item;
								return true;
							}
						}
						return false;
					}
					else
					{
						for (int i = nextIndex; i < 9; i++)
						{
							var item = grid.Elements[i];
							if (!item.HasValue)
							{
								gridElementMap[grid] = i + 1;
								element = item;
								return true;
							}
						}
						return false;
					}
				}
			}

			public void ResetAll()
			{
				lock (this.mapLock)
				{
					var gridElementMap = this.gridElementMap;
					foreach (var grid in this.gridElementMap.Keys.ToArray())
					{
						gridElementMap[grid] = 0;
					}
				}
			}

			public void ResetAllFinished()
			{
				lock (this.mapLock)
				{
					var gridElementMap = this.gridElementMap;
					foreach (var grid in this.gridElementMap
						.Where(item => item.Value > 8)
						.Select(item => item.Key)
						.ToArray())
					{
						gridElementMap[grid] = 0;
					}
				}
			}
		}
	}
}
