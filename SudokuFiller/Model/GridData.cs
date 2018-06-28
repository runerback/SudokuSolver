using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
	public sealed class GridData
	{
		internal GridData(int gridIndex)
		{
			this.index = gridIndex;
			this.elements = Enumerable.Range(0, 9)
				.Select(item => new ElementData(item, gridIndex))
				.ToArray();
		}
		
		private int index;
		public int Index
		{
			get { return this.index; }
		}

		private readonly ElementData[] elements;
		public ElementData[] Elements
		{
			get { return this.elements; }
		}
	}
}
