using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

			Assert.IsFalse(new SudokuValidator().Valdiate(sudoku));
		}

		[TestMethod]
		public void ValidElements()
		{
			var elements = new Definition.Element[9]
			{
				new Definition.Element(),
				new Definition.Element(),
				new Definition.Element(),
				new Definition.Element(),
				new Definition.Element(),
				new Definition.Element(),
				new Definition.Element(),
				new Definition.Element(1),
				new Definition.Element()
			};
			Assert.IsTrue(new SudokuValidator().ValidateElements(elements));
		}
	}
}
