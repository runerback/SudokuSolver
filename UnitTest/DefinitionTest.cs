using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class DefinitionTest
	{
		[TestMethod]
		public void Grid()
		{
			var sudoku = new Definition.Sudoku();
			var grid = sudoku.Grid1;

			for (int i = 0; i < 3; i++)
			{
				for (int j = 1; j <= 3; j++)
				{
					grid.Rows[i].Elements[j - 1].SetValue(3 * i + j);
				}
			}

			for (int i = 0; i < 9; i++)
			{
				Assert.AreEqual(i + 1, (int)grid.Elements[i]);
			}

			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					Assert.AreEqual(j * 3 + i + 1, (int)grid.Columns[i].Elements[j]);
				}
			}

			Console.WriteLine(grid);
		}

		[TestMethod]
		public void Line()
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
			var row147 = Enumerable.Repeat(0, 3)
				.SelectMany(item => Enumerable.Range(1, 3));
			Assert.IsTrue(row147.SequenceEqual(sudoku.Row1.Elements.Select(item => (int)item)));
			Assert.IsTrue(row147.SequenceEqual(sudoku.Row4.Elements.Select(item => (int)item)));
			Assert.IsTrue(row147.SequenceEqual(sudoku.Row7.Elements.Select(item => (int)item)));

			var row258 = Enumerable.Repeat(0, 3)
				.SelectMany(item => Enumerable.Range(4, 3));
			Assert.IsTrue(row258.SequenceEqual(sudoku.Row2.Elements.Select(item => (int)item)));
			Assert.IsTrue(row258.SequenceEqual(sudoku.Row5.Elements.Select(item => (int)item)));
			Assert.IsTrue(row258.SequenceEqual(sudoku.Row8.Elements.Select(item => (int)item)));

			var row369 = Enumerable.Repeat(0, 3)
				.SelectMany(item => Enumerable.Range(7, 3));
			Assert.IsTrue(row369.SequenceEqual(sudoku.Row3.Elements.Select(item => (int)item)));
			Assert.IsTrue(row369.SequenceEqual(sudoku.Row6.Elements.Select(item => (int)item)));
			Assert.IsTrue(row369.SequenceEqual(sudoku.Row9.Elements.Select(item => (int)item)));

			var col147 = Enumerable.Repeat(0, 3)
				.SelectMany(item => Enumerable.Range(0, 3).Select(item1 => item1 * 3 + 1));
			Assert.IsTrue(col147.SequenceEqual(sudoku.Column1.Elements.Select(item => (int)item)));
			Assert.IsTrue(col147.SequenceEqual(sudoku.Column4.Elements.Select(item => (int)item)));
			Assert.IsTrue(col147.SequenceEqual(sudoku.Column7.Elements.Select(item => (int)item)));

			var col258 = Enumerable.Repeat(0, 3)
				.SelectMany(item => Enumerable.Range(0, 3).Select(item1 => item1 * 3 + 2));
			Assert.IsTrue(col258.SequenceEqual(sudoku.Column2.Elements.Select(item => (int)item)));
			Assert.IsTrue(col258.SequenceEqual(sudoku.Column5.Elements.Select(item => (int)item)));
			Assert.IsTrue(col258.SequenceEqual(sudoku.Column8.Elements.Select(item => (int)item)));

			var col369 = Enumerable.Repeat(0, 3)
				.SelectMany(item => Enumerable.Range(1, 3).Select(item1 => item1 * 3));
			Assert.IsTrue(col369.SequenceEqual(sudoku.Column3.Elements.Select(item => (int)item)));
			Assert.IsTrue(col369.SequenceEqual(sudoku.Column6.Elements.Select(item => (int)item)));
			Assert.IsTrue(col369.SequenceEqual(sudoku.Column9.Elements.Select(item => (int)item)));
		}
	}
}
