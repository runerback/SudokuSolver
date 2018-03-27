using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Model
{
	public sealed class Element : ViewModelBase
	{
		public Element(int index)
		{
			this.index = index;
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		private int? value;
		public int? Value
		{
			get { return this.value; }
			internal set
			{
				if (value != this.value)
				{
					this.value = value;
				}
			}
		}
	}
}
