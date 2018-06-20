using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	static class SudokuParseAndResolve
	{
		public static void Resolve()
		{
			Definition.Sudoku sudoku;
			if (!InputSudoku(out sudoku))
				return;
			TrySolve(sudoku, true);
		}

		static bool InputSudoku(out Definition.Sudoku sudoku)
		{
			sudoku = null;

			Console.WriteLine("input sudoku data:");
			Console.WriteLine();
			Console.WriteLine("(whitespace to separate elements");
			Console.WriteLine(" _ to indicate empty element");
			Console.WriteLine(" new line to separete rows");
			Console.WriteLine(" esc to cancel)");
			Console.WriteLine();

			bool cancelled = false;
			int size = SudokuHelper.SUDOKU_SPLIT_LEN;
			int[] values = new int[size * size];

			for (int i = 0; i < size; )
			{
				Console.WriteLine("line " + (i + 1));
				StringBuilder lineBuilder = new StringBuilder();
				bool endOfLine = false;
				while (!endOfLine && !cancelled)
				{
					var key = Console.ReadKey();
					switch (key.Key)
					{
						case ConsoleKey.Enter:
							endOfLine = true;
							break;
						case ConsoleKey.Escape:
							cancelled = true;
							break;
						case ConsoleKey.Backspace:
							if (lineBuilder.Length > 0)
								lineBuilder.Remove(lineBuilder.Length - 1, 1);
							break;
					}

					switch (key.KeyChar)
					{
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
						case '_':
						case ' ':
							lineBuilder.Append(key.KeyChar);
							break;
						default: break;
					}
				}

				if (cancelled)
				{
					Console.WriteLine("cancelled");
					return false;
				}

				int[] lineValues;
				if (!SudokuHelper.TryParse(lineBuilder.ToString(), out lineValues))
				{
					Console.WriteLine("wrong line data");
					i--;
					continue;
				}

				Array.Copy(lineValues, 0, values, i++ * size, size);

				Console.WriteLine();
			}

			sudoku = new Definition.Sudoku();
			using (var sudokuElementIterator = new SudokuElementEnumerable(sudoku, SudokuElementTravelType.Row).GetEnumerator())
			using (var valueIterator = values.AsEnumerable().GetEnumerator())
			{
				while (sudokuElementIterator.MoveNext() && valueIterator.MoveNext())
				{
					var element = sudokuElementIterator.Current;
					int value = valueIterator.Current;
					if (value > 0)
						element.SetValue(value);
				}
			}

			if (!sudoku.Validate())
			{
				sudoku = null;
				Console.WriteLine("invalid sudoku data");
				return false;
			}

			return true;
		}

		private static GUI.SudokuPlayerController playerGUIController = null;

		private static void TrySolve(Definition.Sudoku playingSudoku, bool showGUI)
		{
			SudokuConsole.Print(playingSudoku);

			Console.WriteLine();

			//show sudoku with GUI
			if (showGUI)
				playerGUIController =  new GUI.SudokuPlayerController(null, playingSudoku);

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
			}
			SudokuConsole.Print(playingSudoku);

			Console.WriteLine();

			if (showGUI)
			{
				playerGUIController.Show();
			}
		}

		public static void Dispose()
		{
			if (playerGUIController != null && !playerGUIController.IsClosed)
				playerGUIController.Shutdown();
		}
	}
}
