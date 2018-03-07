using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class SudokuValidator
	{
		public bool Valdiate(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			foreach (var grid in sudoku.Grids)
			{
				if (!ValidateElements(grid.Elements))
					return false;
			}
			foreach (var row in sudoku.Rows)
			{
				if (!ValidateElements(row.Elements))
					return false;
			}
			foreach (var column in sudoku.Columns)
			{
				if (!ValidateElements(column.Elements))
					return false;
			}

			return true;
		}

		public bool ValidateElements(IEnumerable<Definition.Element> elements)
		{
			if (elements == null)
				return false;

			int value = 0;
			bool matched = false;
			foreach (int element in elements
				.Where(element => element.HasValue)
				.OrderBy(element => element))
			{
				while (value++ < 9)
				{
					if (element == value)
					{
						value = element;
						matched = true;
						break;
					}
				}

				if (matched)
				{
					matched = false;
					continue;
				}

				return false;
			}
			return true;
		}
	}
}
