using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Model
{
	public class Sudoku
	{
		public Sudoku()
		{
			this.grids = Enumerable.Range(0, 9)
				.Select(item => new Grid(item))
				.ToArray();
		}

		private Grid[] grids;
		public Grid[] Grids
		{
			get { return this.grids; }
		}

		internal void Sync(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			var grids = this.grids;

			foreach (var grid in sudoku.Grids)
			{
				foreach (var element in grid.Elements)
				{
					grids[element.GridIndex].Elements[element.Index].Value = element.Value;
				}
			}
		}
	}
}
