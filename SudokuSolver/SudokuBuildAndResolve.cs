using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	static class SudokuBuildAndResolve
	{
		/// <summary>
		/// build sudoku from completed sudoku then try to resolve
		/// </summary>
		/// <param name="seed">known seeds to generate a completed sudoku</param>
		/// <param name="showGUI">whether to show GUI. turned on this will make solve time more longer</param>
		public static void BuildAndResolve(int seed, bool showGUI)
		{
			//create the only one instance of Sudoku
			var sudoku = new Definition.Sudoku();

			//build completed Sudoku with one of known seeds
			if (new Core.CompletedSudokuBuilder(seed).Build(sudoku))
			{
				Console.WriteLine("Built");
			}
			else
			{
				SudokuConsole.WaitLine();
				return;
			}

			SudokuConsole.Print(sudoku);

			var builder = new Core.SudokuBuilder(sudoku.Copy(),
				//DateTime.Now.Millisecond);
				0); //use fixed seed so each level build same sudoku

			Console.WriteLine("press Enter to start");
			SudokuConsole.WaitLine();
			Console.Clear();

			int startLevel = 125;
			int lastLevel = 53; //last 53
			for (int i = startLevel; i < lastLevel; i++)
			{
				BuildSudokuAndTrySolve(builder, i, showGUI);

				Console.WriteLine("press Enter to next");
				SudokuConsole.WaitLine();
				Console.Clear();
			}

			BuildSudokuAndTrySolve(builder, lastLevel, showGUI);

			Console.WriteLine("press Enter to exit");
			SudokuConsole.WaitLine();

			if (showGUI)
				playerGUIController.Shutdown();
		}

		private static GUI.SudokuPlayerController playerGUIController;

		private static void BuildSudokuAndTrySolve(Core.SudokuBuilder builder, int level, bool showGUI)
		{
			var originSudoku = builder.Source;

			//create a Soduku to play from completed Sudoku with difficult level
			var playingSudoku = builder.Build(new Core.DifficultLevel(level)); //last: 35

			SudokuConsole.Print(playingSudoku);

			Console.WriteLine();

			//show sudoku with GUI
			var playerGUIController = SudokuBuildAndResolve.playerGUIController;
			if (showGUI)
			{
				if (playerGUIController != null)
					playerGUIController.Close();
				playerGUIController = new GUI.SudokuPlayerController(originSudoku, playingSudoku);
				SudokuBuildAndResolve.playerGUIController = playerGUIController;
			}

			bool solved;
			TimeSpan interval;
			using (var solver = new Core.SudokuSolver(playingSudoku))
			{
				DateTime beforeNow = DateTime.Now;
				solved = solver.TrySolve();
				interval = DateTime.Now - beforeNow;
			}

			string resultInfo = solved ?
				"Solved" :
				"Cannot solve this sudoku with current knowledge";
			Console.WriteLine("{0} in {1:0} ms", resultInfo, interval.TotalMilliseconds);

			if (!solved)
			{
				Console.WriteLine("{0} seats remainder",
					new SudokuElementEnumerable(playingSudoku).SeatCount());
			}

			bool reallySolved = solved && playingSudoku.Validate();

			if (!reallySolved)
			{
				if (solved)
				{
					Console.WriteLine("じゃないよ！");
				}
				SudokuConsole.Print(playingSudoku);
			}
			else
			{
				if (!originSudoku.ValueEquals(playingSudoku))
				{
					Console.WriteLine("in a different way");
				}
			}

			Console.WriteLine();

			if (showGUI)
			{
				playerGUIController.Show();
			}
		}
	}
}
