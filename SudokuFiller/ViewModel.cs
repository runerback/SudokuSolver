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

		public ViewModel()
		{
			this.rightCommand = new SimpleCommand(Right);
		}

		private SimpleCommand rightCommand;
		public SimpleCommand RightCommand
		{
			get { return this.rightCommand; }
		}

		private void Right(object obj)
		{
			System.Windows.MessageBox.Show("oh, hi there");
		}
    }
}
