using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal abstract class SudokuSolverPartternBase : IDisposable
	{
		public SudokuSolverPartternBase(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;

			this.observers = registerObservers(sudoku).ToArray();
			this.validator = new SudokuValidator(sudoku);
		}

		protected readonly Definition.Sudoku sudoku;
		private IEnumerable<Observers.ObserverBase> observers;
		private SudokuValidator validator;

		protected abstract IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku);

		public bool HasFailed
		{
			get { return this.validator.HasFailed; }
		}

		public abstract void Fill();

		/// <summary>
		/// how basic of this pattern. 0 means most basic.
		/// </summary>
		public abstract int Index { get; }

		#region IDisposable

		private bool disposed;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				foreach (var observer in this.observers)
				{
					observer.Dispose();
				}
				this.observers = null;
				this.validator.Dispose();
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
