using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Model
{
	public sealed class Element : ViewModelBase, IDisposable
	{
		public Element(Definition.Element difElement)
		{
			if (difElement == null)
				throw new ArgumentNullException("difElement");
			this.element = difElement;

			this.index = element.Index;

			difElement.ValueChanged += onValueChanged;
		}

		private Definition.Element element;

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		public int? Value
		{
			get { return this.element.Value; }
		}

		private void onValueChanged(object sender, EventArgs e)
		{
			//TODO: this one is async, so use `SudokuSolveStep` to update value in `Model.Element` for better step view
			NotifyPropertyChanged("Value"); 
		}

		private bool disposed;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				this.element.ValueChanged -= onValueChanged;
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
