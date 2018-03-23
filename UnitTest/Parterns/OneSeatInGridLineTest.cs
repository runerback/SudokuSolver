using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SudokuSolver.Parterns.UnitTest
{
	[TestClass]
	public class OneSeatInGridLineTest
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

			//foreach (var grid in sudoku.Grids)
			//{
			//	if (grid.Index / 3 == 1)
			//	{
			//		grid.Column1.Element1.ClearValue();
			//		grid.Column3.Element1.ClearValue();
			//	}
			//	else
			//	{
			//		foreach (var element in grid.Column1.Elements)
			//		{
			//			element.ClearValue();
			//		}
			//	}
			//}

			foreach (var element in sudoku.Grid1.Column1.Elements)
			{
				element.ClearValue();
			}
			foreach (var element in sudoku.Grid7.Column1.Elements)
			{
				element.ClearValue();
			}
			sudoku.Grid4.Column1.Element1.ClearValue();

			Console.WriteLine("Hollowed");
			print(sudoku);
		}

		[TestMethod]
		public void SelectManyIndex()
		{
			for (int a = 0; a < 3; a++)
			{
				for (int i = 1, j = i * a * 3; i <= 3; i++, j += 9)
				{
					for (int k = 1; k <= 3; k++)
					{
						int value = j + k;

						//Console.WriteLine("i: {0}", i);
						//Console.WriteLine("k: {0}", k);
						//Console.WriteLine("a: {0}", a);
						//getIndex(value);

						//Console.WriteLine("value: {0}", value);
						//Console.WriteLine();
						//Console.Write("{0} -> {1}, ", value, a + 1);
						Assert.AreEqual(a, getIndex(value));
					}
				}
			}
		}

		private int getIndex(int value)
		{
			//int i = (value - 1) / 9 + 1;
			//Console.WriteLine("result i: {0}", i);
			//int k = (value - 1) % 3 + 1;
			//Console.WriteLine("result k: {0}", k);
			//int a = (value - 1) % 9 / 3;
			//Console.WriteLine("result a: {0}", a);
			return (value - 1) % 9 / 3;
		}

		[TestMethod]
		public void Fill()
		{
			var sudoku = this.sudoku;

			Assert.IsFalse(sudoku.Grid4.Column1.Element1.HasValue);

			new OneSeatInGridLine(sudoku).Fill();

			Assert.IsTrue(sudoku.Grid4.Column1.Element1.HasValue);

			Console.WriteLine("Filled");
			print(sudoku);
		}
	}
}
