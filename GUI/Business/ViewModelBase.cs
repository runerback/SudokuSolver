using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI
{
	internal class ViewModelBase : INotifyPropertyChanged
	{
		protected ViewModelBase() { }

		private PropertyChangedEventHandler propertyChangedHandler;
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add { this.propertyChangedHandler += value; }
			remove { this.propertyChangedHandler -= value; }
		}

		protected void NotifyPropertyChanged(string propertyName)
		{
			if (propertyChangedHandler != null)
				propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
