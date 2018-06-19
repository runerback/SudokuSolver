using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver
{
	static class SudokuSeedsCollector
	{
		public static void Collect()
		{
			//record all seeds which can generate a completed sudoku
			///TODO: need a parallel here

			bool completed = false;

			var cts = new CancellationTokenSource();
			Task.Factory.StartNew(() =>
			{
				DateTime start = DateTime.Now;
				using (var recorder = new CompletedSudokuBuilderSeedRecorder())
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
						if (new Core.CompletedSudokuBuilder(i).Build(new Definition.Sudoku()))
						{
							recorder.Add(i.ToString());
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
