using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal sealed class AllSeatInGridLine : SudokuSolverPartternBase
	{
		public AllSeatInGridLine(Definition.Sudoku sudoku)
			: base(sudoku)
		{
			this.gridRowEnumerable = new GridLineEnumerable(sudoku, Definition.LineType.Row);
			this.gridColumnEnumerable = new GridLineEnumerable(sudoku, Definition.LineType.Column);

			registerObservers(sudoku);
		}

		private GridLineEnumerable gridRowEnumerable;
		private GridLineEnumerable gridColumnEnumerable;

		private void registerObservers(Definition.Sudoku sudoku)
		{
			foreach (var gridRowObserver in gridRowEnumerable
				.Select((item, i) => new Observers.GridLineObserver(item, (i - 1) % 9 / 3, Observers.GridLineObserverMode.OneSeat))
				.Where(item => !item.IsIdle))
			{
				gridRowObserver.Updated += onGridLineUpdated;
			}
			foreach (var gridColumnObserver in gridColumnEnumerable
				.Select((item, i) => new Observers.GridLineObserver(item, (i - 1) % 9 / 3, Observers.GridLineObserverMode.AllSeat))
				.Where(item => !item.IsIdle))
			{
				gridColumnObserver.Updated += onGridLineUpdated;
			}
		}

		private void onGridLineUpdated(object sender, Observers.GridLineUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Line))
				((Observers.GridLineObserver)sender).Updated -= onGridLineUpdated;
		}

		private bool findOnlyOneElement(Definition.GridLine gridLine, out Definition.Element emptyElement)
		{
			Definition.Element target = null;
			foreach (var element in gridLine.Elements
				.Where(item => !item.HasValue))
			{
				if (target == null)
				{
					target = element;
				}
				else
				{
					target = null;
					break;
				}
			}

			emptyElement = target;
			return target != null;
		}

		private bool fillOnlyOneElement(Definition.GridLine gridLine)
		{
			Definition.Element emptyElement = null;
			if (!findOnlyOneElement(gridLine, out emptyElement))
				return false;

			var lineType = gridLine.LineType;

			//get two other grids
			var currentGrid = gridLine.Grid;

			Definition.Grid otherGrid1, otherGrid2;
			GetOtherGrids(currentGrid, lineType, out otherGrid1, out otherGrid2);

			int currentLineIndex = gridLine.Index;

			var otherElementValues1 = GetElementsInOtherGridLine(otherGrid1, currentLineIndex, lineType)
				.Where(item => item.HasValue)
				.Select(item => item.Value.Value);

			var otherElementValues2 = GetElementsInOtherGridLine(otherGrid2, currentLineIndex, lineType)
				.Where(item => item.HasValue)
				.Select(item => item.Value.Value);

			var otherElementValues = otherElementValues1
				.Concat(otherElementValues2);

			var currentElementValues = currentGrid.Elements
				.Where(item => item.HasValue)
				.Select(item => item.Value.Value);
			var exceptResult = otherElementValues.Except(currentElementValues);

			var exceptResultCount = exceptResult.Count();
			if (exceptResultCount == 0)
				return false; //no result

			int value = -1;

			//get value which appeared twice
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
									currentGrid.TryGetGrid(Definition.Direction.Right, out otherGrid1);
									otherGrid1.TryGetGrid(Definition.Direction.Right, out otherGrid2);
								}
								break;
							case 1:
								{
									currentGrid.TryGetGrid(Definition.Direction.Left, out otherGrid1);
									currentGrid.TryGetGrid(Definition.Direction.Right, out otherGrid2);
								}
								break;
							case 2:
								{
									currentGrid.TryGetGrid(Definition.Direction.Left, out otherGrid2);
									otherGrid2.TryGetGrid(Definition.Direction.Left, out otherGrid1);
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
									currentGrid.TryGetGrid(Definition.Direction.Down, out otherGrid1);
									otherGrid1.TryGetGrid(Definition.Direction.Down, out otherGrid2);
								}
								break;
							case 1:
								{
									currentGrid.TryGetGrid(Definition.Direction.Up, out otherGrid1);
									currentGrid.TryGetGrid(Definition.Direction.Down, out otherGrid2);
								}
								break;
							case 2:
								{
									currentGrid.TryGetGrid(Definition.Direction.Up, out otherGrid2);
									otherGrid2.TryGetGrid(Definition.Direction.Up, out otherGrid1);
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
			foreach (var gridLine in gridRowEnumerable.Concat(gridColumnEnumerable))
			{
				fillOnlyOneElement(gridLine);
			}
		}
	}
}
