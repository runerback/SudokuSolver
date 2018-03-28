﻿using System;
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
			var observers = new GridLineEnumerable(sudoku, Definition.LineType.Row)
				.Concat(new GridLineEnumerable(sudoku, Definition.LineType.Column))
				.Select(item => new Observers.GridLineObserver(item, Observers.SeatMode.One))
				.Where(item => !item.IsIdle)
				.ToArray();

			foreach (var gridLineObserver in observers)
			{
				gridLineObserver.Updated += onGridLineUpdated;
			}

			return observers;
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
			GetOtherGrids(currentGrid, lineType, out otherGrid1, out otherGrid2);

			int currentLineIndex = gridLine.Index;

			var otherElementValues1 = GetElementsInOtherGridLine(otherGrid1, currentLineIndex, lineType)
				.Values();

			var otherElementValues2 = GetElementsInOtherGridLine(otherGrid2, currentLineIndex, lineType)
				.Values();

			var currentElementValues = currentGrid.Elements
				.Values();

			var exceptResult = otherElementValues1
				.Concat(otherElementValues2)
				.Except(currentElementValues);

			if (!exceptResult.Any())
				return false; //no result

			int value = -1;

			//get value which appeared both
			var bothOtherElementValues = otherElementValues1.Intersect(otherElementValues2);
			var exactIntersectElementValues = bothOtherElementValues.Intersect(exceptResult);
			foreach (var result in exactIntersectElementValues)
			{
				if (value < 0)
				{
					value = result;
				}
				else
				{
					return false; //cannot get only one result
				}
			}

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
			foreach (var gridLine in new GridLineEnumerable(sudoku, Definition.LineType.Row)
				.Concat(new GridLineEnumerable(sudoku, Definition.LineType.Column)))
			{
				fillOnlyOneElement(gridLine);
			}
		}

	}
}
