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
			var sudoku = new Definition.Sudoku();

			/*
			int maxRetryCount = 1;
			int retryBase = 0;
			int retryCount = 0;

			while (!new CompletedSudokuBuilder(retryCount + retryBase).Build(sudoku) &&
				retryCount++ < maxRetryCount)
			{
				Console.Clear();
			}

			if (retryCount < maxRetryCount)
			{
				Console.WriteLine("Current Seed: {0}", retryCount + retryBase);

				new SudokuPlayer().Print(sudoku);
			}
			else
			{
				Console.WriteLine("Failed");
			}
			*/

			bool completed = false;

			CancellationTokenSource cts = new CancellationTokenSource();
			Task.Factory.StartNew(() =>
			{
				DateTime start = DateTime.Now;
				using (var recorder = CompletedSudokuBuilderSeedRecorder.Instance)
				{
					for (int i = int.MinValue; i < int.MaxValue; i++)
					{
						if (cts.IsCancellationRequested)
						{
							Console.WriteLine();
							Console.WriteLine("Canceled");
							return;
						}

						Console.Write("\r{0}", i);
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
