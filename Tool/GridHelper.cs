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
		public static bool TryGetAdjacentGrid(this Definition.Grid sourceGrid, GridAdjacentDirection direction, out Definition.Grid grid)
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
	}

	public enum GridAdjacentDirection
	{
		Up,
		Down,
		Left,
		Right
	}
}
