using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SudokuSolver.Core;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class ValidationTest
	{
		[TestMethod]
		public void InvalidSudoku()
		{
			var sudoku = new Definition.Sudoku();

			foreach (var grid in sudoku.Grids)
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 1; j <= 3; j++)
					{
						grid.Rows[i].Elements[j - 1].SetValue(3 * i + j);
					}
				}
			}

			Assert.IsFalse(sudoku.Valdiate());
		}

		[TestMethod]
		public void ValidElements()
		{
			var elements = Enumerable.Range(0, 9)
				.Select(item => new Definition.Element(0, item))
				.ToArray();
			elements[7].SetValue(1);
			Assert.IsTrue(elements.Validate());
		}

		[TestMethod]
		public void ValidElementsWithRepeatValue()
		{
			var elements = Enumerable.Range(0, 9)
				.Select(item => new Definition.Element(0, item))
				.ToArray();
			elements[1].SetValue(2);
			elements[3].SetValue(5);
			elements[4].SetValue(3);
			elements[7].SetValue(1);
			elements[8].SetValue(5);
			Assert.IsFalse(elements.Validate());
		}
	}
}
