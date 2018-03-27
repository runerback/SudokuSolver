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

		/*/grid postion
		0 1 2
		3 4 5
		6 7 8
		*/

		private void onGridLineUpdated(object sender, Observers.GridLineUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Line))
				((Observers.GridLineObserver)sender).Updated -= onGridLineUpdated;
		}

		private bool fillOnlyOneElement(Definition.GridLine gridLine)
		{
			//get empty element
			Definition.Element emptyElement = null;
			foreach (var element in gridLine.Elements
				.Where(item => !item.HasValue))
			{
				if (emptyElement == null)
				{
					emptyElement = element;
				}
				else
				{
					return false;
				}
			}

			if (emptyElement == null)
				return true;

			var lineType = gridLine.LineType;

			//get two other grids
			var currentGrid = gridLine.Grid;
			int currentLayer = GetGridLayer(currentGrid.Index, lineType);

			Definition.Grid otherGrid1, otherGrid2;
			switch (lineType)
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

			/*/should appeared in both other elements
			if (exceptResultCount == 1)
				value = exceptResult.First(); //get only one result
			else
			*/

			//do exact intersection
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

		private int GetGridLayer(int currentGridIndex, Definition.LineType gridLineType)
		{
			switch (gridLineType)
			{
				case Definition.LineType.Row:
					return currentGridIndex % 3;
				case Definition.LineType.Column:
					return currentGridIndex / 3;
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
			foreach (var column in gridColumnEnumerable)
			{
				fillOnlyOneElement(column);
			}
		}

		private class GridLineEnumerable : IEnumerable<Definition.GridLine>
		{
			public GridLineEnumerable(Definition.Sudoku sudoku, Definition.LineType lineType)
			{
				if (sudoku == null)
					throw new ArgumentNullException("sudoku");
				this.sudoku = sudoku;

				this.lineType = lineType;
			}

			private Definition.Sudoku sudoku;
			private Definition.LineType lineType;

			IEnumerator<Definition.GridLine> IEnumerable<Definition.GridLine>.GetEnumerator()
			{
				switch (this.lineType)
				{
					case Definition.LineType.Row:
						return TravelRow(this.sudoku).GetEnumerator();
					case Definition.LineType.Column:
						return TravelColumn(this.sudoku).GetEnumerator();
					default: throw new NotImplementedException();
				}
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				switch (this.lineType)
				{
					case Definition.LineType.Row:
						return TravelRow(this.sudoku).GetEnumerator();
					case Definition.LineType.Column:
						return TravelColumn(this.sudoku).GetEnumerator();
					default: throw new NotImplementedException();
				}
			}

			private IEnumerable<Definition.GridLine> TravelRow(Definition.Sudoku sudoku)
			{
				foreach (var grid in sudoku.Grids)
				{
					foreach (var row in grid.Rows)
					{
						yield return row;
					}
				}
			}

			private IEnumerable<Definition.GridLine> TravelColumn(Definition.Sudoku sudoku)
			{
				foreach (var grid in sudoku.Grids)
				{
					foreach (var column in grid.Columns)
					{
						yield return column;
					}
				}
			}
		}
	}
}
