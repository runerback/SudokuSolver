using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuFiller
{
	public enum SudokuSolveState
	{
		UNKNOWN = 0,
		Invalid,
		Completed,
		Unsolved,
		Solved,
	}
}
