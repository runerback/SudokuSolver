using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Parterns.UnitTest
{
	[TestClass]
	public class OnlyOneSeatTest
	{
		private Definition.Sudoku sudoku =
			new Definition.Sudoku();

		private void print(Definition.Sudoku sudoku)
		{
			Console.WriteLine(
				string.Join("\r\n",
					sudoku.Rows.Select(row =>
						string.Join("\t*\t", row.Segments.Select(segment =>
							string.Join("\t", segment.Elements.AsEnumerable()))))));
		}

		[TestInitialize]
		public void Initialize()
		{
			var sudoku = this.sudoku;

			Assert.IsTrue(new CompletedSudokuBuilder(-1666468307)
				.Build(sudoku));

			Console.WriteLine("Origin");
			print(sudoku);

			foreach (var grid in sudoku.Grids)
			{
				grid.Elements[grid.Index].ClearValue();
			}

			Console.WriteLine("Hollowed");
			print(sudoku);
		}

		[TestMethod]
		public void FillElements()
		{
			int[] elements = new int[8];
			for (int i = 1; i <= 9; i++)
			{
				for (int k = 1, l = 0; k <= 9; k++)
				{
					if (i == k)
						continue;
					elements[l++] = k;
				}

				Assert.AreEqual(i, getTheValue(elements));
			}
		}

		private int getTheValue(IEnumerable<int> values)
		{
			int result = 1;
			foreach (var value in values
				.OrderBy(item => item))
			{
				if (value != result)
					break;
				result++;
			}
			return result;
		}

		[TestMethod]
		public void Fill()
		{
			var sudoku = this.sudoku;

			Assert.IsTrue(new Parterns.OneSeatInNine(sudoku).Fill());
			Assert.IsTrue(new Observers.CompletionStateObserver(sudoku).IsCompleted);
			Console.WriteLine("Filled");
			print(sudoku);
		}
	}
}
