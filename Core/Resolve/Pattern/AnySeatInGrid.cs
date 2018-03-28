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
			GetOtherGrids(grid, Definition.LineType.Column, out otherGridInRow1, out otherGridInRow2);

			//get two other grids in same column
			Definition.Grid otherGridInColumn1, otherGridInColumn2;
			GetOtherGrids(grid, Definition.LineType.Row, out otherGridInColumn1, out otherGridInColumn2);

			int elementIndex = emptyElement.Index;
			int rowLineIndex = elementIndex / 3;
			int columnLineIndex = elementIndex % 3;

			var otherGridInRow1Values = GetElementsInOtherGridLine(otherGridInRow1, rowLineIndex, Definition.LineType.Column)
				.Values();
			var otherGridInRow2Values = GetElementsInOtherGridLine(otherGridInRow2, rowLineIndex, Definition.LineType.Column)
				.Values();
			var otherGridInColumn1Values = GetElementsInOtherGridLine(otherGridInColumn1, rowLineIndex, Definition.LineType.Row)
				.Values();
			var otherGridInColumn2Values = GetElementsInOtherGridLine(otherGridInColumn2, rowLineIndex, Definition.LineType.Row)
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

		private void GetOtherGrids(Definition.Grid currentGrid, Definition.LineType gridLineType, out Definition.Grid otherGrid1, out Definition.Grid otherGrid2)
		{
			int currentLayer = -1;

			switch (gridLineType)
			{
				case Definition.LineType.Row:
					currentLayer = currentGrid.Index % 3;
					break;
				case Definition.LineType.Column:
					currentLayer = currentGrid.Index / 3;
					break;
				default: throw new NotImplementedException();
			}

			switch (gridLineType)
			{
				case Definition.LineType.Row:
					{
						switch (currentLayer)
						{
							case 0:
								{
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Right, out otherGrid1);
									otherGrid1.TryGetAdjacentGrid(GridAdjacentDirection.Right, out otherGrid2);
								}
								break;
							case 1:
								{
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Left, out otherGrid1);
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Right, out otherGrid2);
								}
								break;
							case 2:
								{
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Left, out otherGrid2);
									otherGrid2.TryGetAdjacentGrid(GridAdjacentDirection.Left, out otherGrid1);
								}
								break;
							default: throw new NotImplementedException();
						}

					}
					break;
				case Definition.LineType.Column:
					{
						switch (currentLayer)
						{
							case 0:
								{
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Down, out otherGrid1);
									otherGrid1.TryGetAdjacentGrid(GridAdjacentDirection.Down, out otherGrid2);
								}
								break;
							case 1:
								{
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Up, out otherGrid1);
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Down, out otherGrid2);
								}
								break;
							case 2:
								{
									currentGrid.TryGetAdjacentGrid(GridAdjacentDirection.Up, out otherGrid2);
									otherGrid2.TryGetAdjacentGrid(GridAdjacentDirection.Up, out otherGrid1);
								}
								break;
							default: throw new NotImplementedException();
						}
					}
					break;
				default: throw new NotImplementedException();
			}
		}

		private IEnumerable<Definition.Element> GetElementsInOtherGridLine(Definition.Grid grid, int skipIndex, Definition.LineType type)
		{
			IEnumerable<Definition.GridLine> lines;
			switch (type)
			{
				case Definition.LineType.Row:
					lines = grid.Rows;
					break;
				case Definition.LineType.Column:
					lines = grid.Columns;
					break;
				default: throw new NotImplementedException();
			}
			lines = lines
				.Skip(skipIndex == 0 ? 1 : 0)
				.Take(skipIndex == 1 ? 1 : 2);
			if (skipIndex == 1)
				lines = lines.Skip(1).Take(1);
			return lines.SelectMany(item => item.Elements);
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
