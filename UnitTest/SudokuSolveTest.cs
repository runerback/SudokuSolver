using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class SudokuSolveTest
	{
		[TestMethod]
		public void ParseFromString()
		{
			string source = @"7 9 5 1 2 4 3 6 8
1 2 4 8 6 3 7 9 5
3 6 8 7 5 9 2 1 4
6 4 1 _ _ _ _ 8 7
2 5 9 _ 7 8 6 4 _
_ 8 7 _ _ _ _ 2 _
_ 1 _ 5 _ 7 _ 3 6
_ 7 _ _ _ _ _ 5 _
5 3 _ _ 8 _ 1 7 _";

			Definition.Sudoku sudoku;
			Assert.IsTrue(SudokuHelper.TryParse(source, out sudoku));
		}

		[TestMethod]
		public void Test54SeatsError()
		{
			string source = @"7 9 5 1 4 2 3 6 8
1 2 4 8 6 3 7 9 5
8 3 6 7 _ _ 2 1 4
_ 4 1 _ _ _ 5 8 7
_ 5 9 _ _ 8 6 4 1
_ 8 _ _ _ _ 9 2 3
9 _ 8 5 _ _ 4 3 6
4 _ _ _ _ _ 8 5 2
5 _ _ _ 8 _ 1 7 9";

			Definition.Sudoku sudoku;
			Assert.IsTrue(SudokuHelper.TryParse(source, out sudoku));
			Assert.IsTrue(sudoku.Valdiate(), "Invalid sudoku");

			var sourceSudoku = sudoku.Copy();
			var guiController = new GUI.SudokuPlayerController(sourceSudoku, sudoku);

			using (var solver = new Core.SudokuSolver(sudoku))
			{
				Assert.IsTrue(solver.TrySolve());
			}

			guiController.ShowAndWaitForClose();
		}
	}
}
