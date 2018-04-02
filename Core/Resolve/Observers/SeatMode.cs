using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal enum SeatMode
	{
		/// <summary>
		/// only one seat
		/// </summary>
		One = 1,

		/// <summary>
		/// only two seats
		/// </summary>
		Two = 2,

		/// <summary>
		/// any seat
		/// </summary>
		All = 9
	}
}
