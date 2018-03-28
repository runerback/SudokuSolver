﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal class UpdatedEventArgs : EventArgs
	{
		public UpdatedEventArgs(int index)
		{
			this.index = index;
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}
	}
}
