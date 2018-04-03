using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static class GridHelper
	{
		/// <summary>
		/// get adjacent grid by direction
		/// </summary>
		/// <remarks>
		/// 0 1 2
		/// 3 4 5
		/// 6 7 8
		/// </remarks>
		private static bool TryGetAdjacentGrid(this Definition.Grid sourceGrid, GridAdjacentDirection direction, out Definition.Grid grid)
		{
			int index = sourceGrid.Index;
			int targetIndex = -1;

			switch (direction)
			{
				case GridAdjacentDirection.Up:
					{
						targetIndex = index - 3;
					}
					break;
				case GridAdjacentDirection.Down:
					{
						targetIndex = index + 3;
					}
					break;
				case GridAdjacentDirection.Left:
					{
						if (index % 3 > 0)
						{
							targetIndex = index - 1;
						}
					}
					break;
				case GridAdjacentDirection.Right:
					{
						if (index % 3 < 2)
						{
							targetIndex = index + 1;
						}
					}
					break;
				default: throw new NotImplementedException();
			}

			if (targetIndex < 0)
			{
				grid = null;
				return false;
			}
			else
			{
				grid = sourceGrid.Sudoku.Grids[targetIndex];
				return true;
			}
		}

		public static void GetOtherGrids(this Definition.Grid sourceGrid, Definition.LineType gridLineType, out Definition.Grid otherGrid1, out Definition.Grid otherGrid2)
		{
			int currentLayer = GetLayerWithOppositeLineType(sourceGrid.Index, gridLineType);

			switch (gridLineType)
			{
				case Definition.LineType.Row:
					{
						switch (currentLayer)
						{
							case 0:
								{
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Right, out otherGrid1);
									otherGrid1.TryGetAdjacentGrid(GridAdjacentDirection.Right, out otherGrid2);
								}
								break;
							case 1:
								{
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Left, out otherGrid1);
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Right, out otherGrid2);
								}
								break;
							case 2:
								{
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Left, out otherGrid2);
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
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Down, out otherGrid1);
									otherGrid1.TryGetAdjacentGrid(GridAdjacentDirection.Down, out otherGrid2);
								}
								break;
							case 1:
								{
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Up, out otherGrid1);
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Down, out otherGrid2);
								}
								break;
							case 2:
								{
									sourceGrid.TryGetAdjacentGrid(GridAdjacentDirection.Up, out otherGrid2);
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

		/// <param name="skipIndex">line index to skip</param>
		public static IEnumerable<Definition.Element> GetElementsInOtherGridLine(this Definition.Grid sourceGrid, int skipIndex, Definition.LineType lineType)
		{
			IEnumerable<Definition.GridLine> lines;
			switch (lineType)
			{
				case Definition.LineType.Row:
					lines = sourceGrid.Rows;
					break;
				case Definition.LineType.Column:
					lines = sourceGrid.Columns;
					break;
				default: throw new NotImplementedException();
			}

			foreach (var line in lines)
			{
				if (line.Index != skipIndex)
				{
					foreach (var element in line.Elements)
					{
						yield return element;
					}
				}
			}
		}

		public static IEnumerable<Definition.Element> GetElementsInCurrentGridLine(this Definition.Grid sourceGrid, int layerIndex, Definition.LineType lineType)
		{
			IEnumerable<Definition.GridLine> lines;
			switch (lineType)
			{
				case Definition.LineType.Row:
					lines = sourceGrid.Rows;
					break;
				case Definition.LineType.Column:
					lines = sourceGrid.Columns;
					break;
				default: throw new NotImplementedException();
			}

			return lines.Skip(layerIndex).First().Elements;
		}

		private static int GetLayerWithOppositeLineType(int index, Definition.LineType oppositeLineType)
		{
			switch (oppositeLineType)
			{
				case Definition.LineType.Row: return index % 3;
				case Definition.LineType.Column: return index / 3;
				default: throw new NotImplementedException();
			}
		}

		public static int GetLayer(int index, Definition.LineType lineType)
		{
			switch (lineType)
			{
				case Definition.LineType.Row: return index / 3;
				case Definition.LineType.Column: return index % 3;
				default: throw new NotImplementedException();
			}
		}

		public static Definition.LineType GetLineType(int index1, int index2)
		{
			if (index1 == index2)
				throw new InvalidOperationException("same index");
			return (index1 - index2) % 3 == 0 ? Definition.LineType.Column : Definition.LineType.Row;
		}
	}

	public enum GridAdjacentDirection
	{
		Up,
		Down,
		Left,
		Right
	}
}
