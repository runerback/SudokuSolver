using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class SudokuPlayer
	{
		private const char grid_horizontal = (char)0x2500;
		private const char horizontal = (char)0x2501;

		private const char grid_vertical = (char)0x2502;
		private const char vertical = (char)0x2503;

		private const char grid_left_top = (char)0x250C;
		private const char left_top = (char)0x250F;

		private const char grid_right_top = (char)0x2510;
		private const char right_top = (char)0x2513;

		private const char grid_left_bottom = (char)0x2514;
		private const char left_bottom = (char)0x2517;

		private const char grid_right_bottom = (char)0x2518;
		private const char right_bottom = (char)0x251B;

		private const char grid_top_cross = (char)0x252C;
		private const char grid_left_cross = (char)0x251C;
		private const char grid_right_cross = (char)0x2524;
		private const char grid_bottom_cross = (char)0x2534;
		private const char grid_cross = (char)0x253C;

		private const char whitespace = ' ';

		public void Print(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			int gridIndex = 0;
			var cursorTop = Console.CursorTop;
			foreach (var grid in sudoku.Grids)
			{
				printGrid(grid, gridIndex++, cursorTop);
			}
			Console.WriteLine();
		}

		private void printGrid(Definition.Grid grid, int gridIndex, int currentCursorTop)
		{
			if (grid == null)
				throw new ArgumentNullException("grid");

			int cursorPosH = gridIndex % 3 * 10;
			int cursorPosV = gridIndex / 3 * 4 + currentCursorTop;

			bool firstColumn = gridIndex % 3 == 0;
			bool lastColumn = gridIndex % 3 == 2;

			bool firstRow = gridIndex < 3;
			bool lastRow = gridIndex > 5;

			if (lastColumn)
				cursorPosH--;

			if (firstRow)
			{
				Console.SetCursorPosition(lastColumn ? cursorPosH - 1 : cursorPosH, cursorPosV + 0);
				if (firstColumn)
					Console.Write(grid_left_top);
				for (int i = 0; i < 3; i++)
				{
					Console.Write(grid_horizontal);
				}
				Console.Write(lastColumn ? grid_right_top : grid_top_cross);
			}

			Console.SetCursorPosition(cursorPosH, cursorPosV + 1);
			if (firstColumn)
				Console.Write(grid_vertical);
			if (!lastColumn)
				Console.Write(whitespace);
			Console.Write(grid.Row1);
			Console.Write(grid_vertical);

			Console.SetCursorPosition(cursorPosH, cursorPosV + 2);
			if (firstColumn)
				Console.Write(grid_vertical);
			if (!lastColumn)
				Console.Write(whitespace);
			Console.Write(grid.Row2);
			Console.Write(grid_vertical);

			Console.SetCursorPosition(cursorPosH, cursorPosV + 3);
			if (firstColumn)
				Console.Write(grid_vertical);
			if (!lastColumn)
				Console.Write(whitespace);
			Console.Write(grid.Row3);
			Console.Write(grid_vertical);

			Console.SetCursorPosition(lastColumn ? cursorPosH - 1 : cursorPosH, cursorPosV + 4);
			if(firstColumn)
				Console.Write(lastRow?grid_left_bottom:grid_left_cross);
			for (int i = 0; i < 3; i++)
			{
				Console.Write(grid_horizontal);
			}
			Console.Write(lastColumn ?
				(lastRow ? grid_right_bottom : grid_right_cross) :
				(lastRow ? grid_bottom_cross : grid_cross));
		}
	}
}
