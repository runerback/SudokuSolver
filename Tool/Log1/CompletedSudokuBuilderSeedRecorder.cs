using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SudokuSolver
{
	public sealed class CompletedSudokuBuilderSeedRecorder : Log
	{
		public CompletedSudokuBuilderSeedRecorder()
			: base("Completed Sudoku Builder Seeds.log")
		{
		}

		public int GetLastRecord()
		{
			string recordLine = null;
			bool locked = false;
			foreach (var line in new MiscUtil.IO.ReverseLineReader(
				() => new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
				Encoding))
			{
				if (!locked && !base.TestLogStreamLock())
				{
					locked = true;
					continue;
				}
				recordLine = line;
				break;
			}
			return int.Parse(recordLine);
		}

		protected override void Disposing()
		{
		}
	}
}
