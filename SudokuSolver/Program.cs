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
			if (new CompletedSudokuBuilder(-1666468307).Build(sudoku))
			{
				Console.WriteLine("Built");
			}
			else
			{
				Console.ReadLine();
				return;
			}

			var player = new SudokuPlayer();

			player.Print(sudoku);

			var builder = new SudokuBuilder(sudoku, DateTime.Now.Millisecond);

			//create a Soduku to play from completed Sudoku with difficult level
			var playingSudoku = builder.Build(new DifficultLevel(13));

			player.Print(playingSudoku);

			var resolver = new SudokuResolver(playingSudoku);

			DateTime beforeNow = DateTime.Now;
			string resultInfo = resolver.TryResolve() ? 
				"Solved" : 
				"Cannot solve this sudoku with current knowledge";
			Console.WriteLine("{0} in {1} ms", resultInfo, (DateTime.Now - beforeNow).TotalMilliseconds);

			player.Print(playingSudoku);

			if (!new SudokuValidator().Valdiate(playingSudoku))
				Console.WriteLine("じゃないよ！");

			Console.ReadLine();
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
				using (var recorder = CompletedSudokuBuilderSeedRecorder.Instance)
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
						if (new CompletedSudokuBuilder(i).Build(sudoku))
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
