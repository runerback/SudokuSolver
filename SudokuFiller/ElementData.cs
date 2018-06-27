using Runerback.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
	public sealed class ElementData : ViewModelBase
	{
		internal ElementData(int elementIndex)
		{
			this.index = elementIndex;
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		private int? value = null;
		public int? Value
		{
			get { return this.value; }
			set
			{
				if (value != this.value)
				{
					this.value = value;
					NotifyPropertyChanged("Value");
				}
			}
		}
	}
}
