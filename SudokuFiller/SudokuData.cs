using Runerback.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
	public sealed class SudokuData : ViewModelBase
	{
		public SudokuData()
		{
			this.grids = Enumerable.Range(0, 9)
				.Select(item => new GridData(item))
				.ToArray();
			this.inputValidator = new SudokuInputValidator(this);
		}

		private readonly GridData[] grids;
		public GridData[] Grids
		{
			get { return this.grids; }
		}

		private bool isInputMode = true;
		public bool IsInputMode
		{
			get { return this.isInputMode; }
		}

		public Task Solve()
		{
			return Task.Factory.StartNew(() =>
			{
				exitInputMode();
				throw new NotImplementedException();
			});
		}

		private SudokuInputValidator inputValidator;

		private void exitInputMode()
		{
			this.inputValidator.Dispose();
			this.inputValidator = null;

			this.isInputMode = false;
			NotifyPropertyChanged("IsInputMode");
		}

		public IEnumerable<int?> Values()
		{
			foreach (var grid in grids)
				foreach (var element in grid.Elements)
					yield return element.Value;
		}
	}
}
