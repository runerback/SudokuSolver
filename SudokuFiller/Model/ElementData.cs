using Runerback.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
	public sealed class ElementData : ViewModelBase
	{
		internal ElementData(int elementIndex, int gridIndex)
		{
			this.index = elementIndex;
			this.gridIndex = gridIndex;
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		private int gridIndex;
		public int GridIndex
		{
			get { return this.gridIndex; }
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

		public bool IsEmpty
		{
			get { return !this.value.HasValue; }
		}

		#region Error

		private string error;
		public string Error
		{
			get { return error; }
		}

		private bool hasError;
		public bool HasError
		{
			get { return !string.IsNullOrEmpty(error); }
		}

		internal void SetError(string value)
		{
			if (this.error != value)
			{
				if (string.IsNullOrEmpty(value))
				{
					this.error = null;
					this.hasError = false;
				}
				else
				{
					this.error = value;
					this.hasError = true;

					Console.Write(value);
				}
				NotifyPropertyChanged("Error");
				NotifyPropertyChanged("HasError");
			}
		}

		internal void ClearError()
		{
			SetError(null);
		}

		#endregion Error

	}
}
