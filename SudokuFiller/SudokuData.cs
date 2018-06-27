using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
	public sealed class SudokuData
	{
		public SudokuData()
		{
			this.grids = Enumerable.Range(0, 9)
				.Select(item => new GridData(item))
				.ToArray();
		}

		private readonly GridData[] grids;
		public GridData[] Grids
		{
			get { return this.grids; }
		}

		public IEnumerable<int?> Values()
		{
			foreach (var grid in grids)
				foreach (var element in grid.Elements)
					yield return element.Value;
		}
	}
}
