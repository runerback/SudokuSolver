using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver
{
	class Program
	{
		static void Main(string[] args)
		{
			bool showGUI = false; //turned on will make solve time wrong (more longer)

			//create the only one instance of Sudoku
			var sudoku = new Definition.Sudoku();

			//build completed Sudoku with one of known seeds
			if (new Core.CompletedSudokuBuilder(-1666468307).Build(sudoku))
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
				0); //use fixed seed so each level match to only one sudoku

			/*
			//create a Soduku to play from completed Sudoku with difficult level
			var playingSudoku = builder.Build(new Core.DifficultLevel(29));

			SudokuConsole.Print(playingSudoku);

			Console.WriteLine();

			//show sudoku with GUI
			GUI.SudokuPlayerController playerGUIController = null;
			if (showGUI) 
				playerGUIController = GUI.SudokuPlayer.Show(sudoku, playingSudoku);

			var solver = new Core.SudokuSolver(playingSudoku);

			DateTime beforeNow = DateTime.Now;
			bool solved = solver.TrySolve();
			TimeSpan interval = DateTime.Now - beforeNow;

			string resultInfo = solved ? 
				"Solved" : 
				"Cannot solve this sudoku with current knowledge";
			Console.WriteLine("{0} in {1:0} ms", resultInfo, interval.TotalMilliseconds);

			bool reallySolved = solved && new Core.SudokuValidator().Valdiate(playingSudoku);

			if (!reallySolved)
			{
				if (solved)
				{
					Console.WriteLine("じゃないよ！");
				}
				SudokuConsole.Print(playingSudoku);
			}

			Console.WriteLine();

			if (showGUI)
			{
				ShowGUI(playerGUIController);
			}
			else
			{
				Console.WriteLine("press Enter to exit");
				SudokuConsole.WaitLine();
			}
			*/

			Console.WriteLine("press Enter to start");
			SudokuConsole.WaitLine();
			Console.Clear();

			int startLevel = 100;
			int lastLevel = 49;
			for (int i = startLevel; i < lastLevel; i++)
			{
				TrySolveSudoku(builder, i, showGUI);

				Console.WriteLine("press Enter to next");
				SudokuConsole.WaitLine();
				Console.Clear();
			}

			TrySolveSudoku(builder, lastLevel, showGUI);
			
			Console.WriteLine("press Enter to exit");
			SudokuConsole.WaitLine();

			return;

			CollectSudokuSeeds(sudoku);
		}

		private static void CollectSudokuSeeds(Definition.Sudoku sudoku)
		{
			//record all seeds which can generate a completed sudoku
			///TODO: need a parallel here

			bool completed = false;

			CancellationTokenSource cts = new CancellationTokenSource();
			Task.Factory.StartNew(() =>
			{
				DateTime start = DateTime.Now;
				using (var recorder = Core.CompletedSudokuBuilderSeedRecorder.Instance)
				{
					int lastRecord =
						//-1666453861;
						recorder.GetLastRecord();

					for (int i = lastRecord; i < int.MaxValue; i++)
					{
						if (cts.IsCancellationRequested)
						{
							Console.WriteLine();
							Console.WriteLine("Canceled");
							return;
						}

						//Console.Write("\r{0}", i);
						if (new Core.CompletedSudokuBuilder(i).Build(sudoku))
						{
							recorder.Add(i);
						}
					}
				}
				DateTime end = DateTime.Now;
				Console.WriteLine();
				Console.WriteLine("Completed in {0:c}", end - start);
				completed = true;
			}
			, cts.Token)
			.ContinueWith(t =>
			{
				cts.Dispose();
			});

			Console.ReadLine();
			if (!completed)
			{
				cts.Cancel();
				Console.ReadLine();
			}
		}

		private static void TrySolveSudoku(Core.SudokuBuilder builder, int level, bool showGUI)
		{
			//create a Soduku to play from completed Sudoku with difficult level
			var playingSudoku = builder.Build(new Core.DifficultLevel(level)); //last: 35

			SudokuConsole.Print(playingSudoku);

			Console.WriteLine();

			//show sudoku with GUI
			GUI.SudokuPlayerController playerGUIController = null;
			if (showGUI)
				playerGUIController = GUI.SudokuPlayer.Show(builder.Source, playingSudoku);

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
					playingSudoku.Grids.SelectMany(grid => grid.Elements).Count(element => !element.HasValue));
			}

			bool reallySolved = solved && new Core.SudokuValidator().Valdiate(playingSudoku);

			if (!reallySolved)
			{
				if (solved)
				{
					Console.WriteLine("じゃないよ！");
				}
				SudokuConsole.Print(playingSudoku);
			}

			Console.WriteLine();

			if (showGUI)
			{
				ShowGUI(playerGUIController);
			}
		}

		private static void ShowGUI(GUI.SudokuPlayerController controller)
		{
			Console.WriteLine("--------------GUI Control----------------");
			Console.WriteLine("|	→ show next step		|");
			Console.WriteLine("|	← show previous step		|");
			Console.WriteLine("|	 ┙exit				|");
			Console.WriteLine("-----------------------------------------");

			while (true)
			{
				var key = SudokuConsole.GetKey();

				if (key == ConsoleKey.Enter)
					break;

				if (key == ConsoleKey.LeftArrow)
				{
					if (!controller.ShowPreviousStep())
						Console.WriteLine("No more steps");
				}
				else if (key == ConsoleKey.RightArrow)
				{
					if (!controller.ShowNextStep())
						Console.WriteLine("No more steps");
				}
			}
			controller.Close();
		}
	}
}
