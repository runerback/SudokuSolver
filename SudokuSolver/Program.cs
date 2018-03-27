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
			//create the only one instance of Sudoku
			var sudoku = new Definition.Sudoku();

			//build completed Sudoku with one of known seeds
			if (new Core.CompletedSudokuBuilder(-1666468307).Build(sudoku))
			{
				Console.WriteLine("Built");
			}
			else
			{
				Extension.SudokuSolverExtension.WaitForLine();
				return;
			}

			var player = new SudokuPlayer();

			player.Print(sudoku);

			var builder = new Core.SudokuBuilder(sudoku.Copy(),
				//DateTime.Now.Millisecond);
				0); //use fixed seed so each level match to only one sudoku

			//create a Soduku to play from completed Sudoku with difficult level
			var playingSudoku = builder.Build(new Core.DifficultLevel(24));

			player.Print(playingSudoku);

			//show sudoku with GUI
			var playerGUIController = GUI.SudokuPlayer.Show(sudoku, playingSudoku);

			var resolver = new Core.SudokuResolver(playingSudoku);

			DateTime beforeNow = DateTime.Now;
			string resultInfo = resolver.TryResolve() ? 
				"Solved" : 
				"Cannot solve this sudoku with current knowledge";
			Console.WriteLine("{0} in {1} ms", resultInfo, (DateTime.Now - beforeNow).TotalMilliseconds);

			player.Print(playingSudoku);

			if (!new Core.SudokuValidator().Valdiate(playingSudoku))
				Console.WriteLine("じゃないよ！");

			Console.WriteLine("press Enter to show steps...");
			Extension.SudokuSolverExtension.WaitForLine();
			while (playerGUIController.PlayNext())
			{
				Extension.SudokuSolverExtension.WaitForLine();
			}

			Console.WriteLine("press Enter to continue...");
			Extension.SudokuSolverExtension.WaitForLine();

			playerGUIController.Close();
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
	}
}
