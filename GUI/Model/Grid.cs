using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Model
{
	internal sealed class Grid
	{
		public Grid(int gridIndex)
		{
			this.index = gridIndex;

			this.elements = Enumerable.Range(0, 9)
				.Select(item => new Element(item))
				.ToArray();
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		private Element[] elements;
		public Element[] Elements
		{
			get { return this.elements; }
		}
	}
}
