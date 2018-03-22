using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class DifficultLevel
	{
		private const int TOTAL = 81;
		private const int MIN = TOTAL - 80;
		private const int MAX = TOTAL - 17;

		public DifficultLevel(int level)
		{
			if (level < MIN)
				throw new ArgumentOutOfRangeException("level. shoud greater than "+MIN);
			if (level > MAX)
				throw new ArgumentOutOfRangeException("level. should less than " + MAX);
			this.level = level;
			this.difficulty = string.Format("{0:0.00} %", (double)level / (double)MAX * 100);
		}

		public static readonly DifficultLevel Max = new DifficultLevel(MAX);
		public static readonly DifficultLevel Min = new DifficultLevel(MIN);

		private int level;
		public int Level
		{
			get { return level; }
		}

		private string difficulty;
		public string Difficulty
		{
			get { return this.difficulty; }
		}
	}
}
