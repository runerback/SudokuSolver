using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core
{
	public class SudokuBuilder
	{
		public SudokuBuilder(Definition.Sudoku completedSudoku, int seed)
		{
			this.rnd = new Random(seed);

			if (completedSudoku == null)
				throw new ArgumentNullException("completedSudoku");
			this.source = completedSudoku;
		}

		private Random rnd;

		private readonly Definition.Sudoku source;
		public Definition.Sudoku Source
		{
			get { return source; }
		}

		private const int TOTAL = 81;
		private const int BLOCK_LEN = 9;

		private int GetNextPosition()
		{
			return this.rnd.Next(TOTAL);
		}

		private Definition.Element GetNextElement(Definition.Sudoku targetSudoku)
		{
			int nextPos = GetNextPosition();
			int discuss = nextPos / BLOCK_LEN;
			int reminder = nextPos % BLOCK_LEN;

			return targetSudoku.Grids[discuss].Elements[reminder];
		}

		public Definition.Sudoku Build(DifficultLevel difficultLevel)
		{
			if (difficultLevel == null)
				throw new ArgumentNullException("difficultLevel");

			Console.WriteLine("Difficulty: {0}", difficultLevel.Difficulty);

			int level = difficultLevel.Level;

			Definition.Sudoku targetSudoku = this.source.Copy();

			int seatsCount = 0;

			for (int i = 0; i < level; i++)
			{
				while (true)
				{
					var element = GetNextElement(targetSudoku);
					if (element.HasValue)
					{
						element.ClearValue();
						seatsCount++;
						break;
					}
				}
			}

			if (seatsCount != level)
				throw new Exception(
					string.Format("Expected:\t{0} seats\r\nActually:\t{1} seats", level, seatsCount));

			return targetSudoku;
		}
	}
}
