using Runerback.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
    sealed class ViewModel : ViewModelBase
    {
		private readonly SudokuData sudokuData = new SudokuData();
		public SudokuData SudokuData
		{
			get { return this.sudokuData; }
		}
    }
}
