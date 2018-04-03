using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Business
{
	internal abstract class SudokuValueSyncBase : IDisposable
	{
		public SudokuValueSyncBase(Definition.Sudoku defSudoku, Model.Sudoku sudoku)
		{
			if (defSudoku == null)
				throw new ArgumentNullException("defSudoku");
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			this.defSudoku = defSudoku;
			this.sudoku = sudoku;

			foreach (var grid in defSudoku.Grids)
			{
				foreach (var element in grid.Elements)
				{
					element.ValueChanged += onElementValueChanged;
				}
			}
		}

		private Definition.Sudoku defSudoku;
		private Model.Sudoku sudoku;

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var defElement = (Definition.Element)sender;
			var element = this.sudoku.Grids[defElement.GridIndex].Elements[defElement.Index];
			onElementValueChanged(element, defElement.HasValue, defElement.Value);
		}

		protected abstract void onElementValueChanged(Model.Element element, bool hasValue, int newValue);

		#region IDisposable

		private bool disposed;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				foreach (var grid in defSudoku.Grids)
				{
					foreach (var element in grid.Elements)
					{
						element.ValueChanged -= onElementValueChanged;
					}
				}
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable

	}
}
