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

			//record all seeds which can generate a completed sudoku
			///TODO: need a parallel here

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
