using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Model
{
	public class Sudoku : IDisposable
	{
		public Sudoku(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			this.grids = sudoku.Grids
				.Select(item => new Grid(item))
				.ToArray();
		}

		private Grid[] grids;
		public Grid[] Grids
		{
			get { return this.grids; }
		}

		private bool disposed;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				foreach (var grid in this.grids)
				{
					grid.Dispose();
				}
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
