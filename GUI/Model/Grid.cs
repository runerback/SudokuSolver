using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Model
{
	public sealed class Grid : IDisposable
	{
		public Grid(Definition.Grid grid)
		{
			if (grid == null)
				throw new ArgumentNullException("grid");
			this.index = grid.Index;

			this.elements = grid.Elements
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

		private bool disposed;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				foreach (var element in this.elements)
				{
					element.Dispose();
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
