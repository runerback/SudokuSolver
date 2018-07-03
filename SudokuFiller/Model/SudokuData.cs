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

		public SudokuSolveResult Solve()
		{
			bool validationFailed = this.inputValidator.HasError;

			exitInputMode();

			if (validationFailed)
				return SudokuSolveResult.Invalid;
			if (!this.Values().Any(item => !item.HasValue))
				return SudokuSolveResult.Completed;
			return new SudokuSolverWithSteps(this).Solve();
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

		public SudokuData Copy()
		{
			var copy = new SudokuData();

			using (var elementIterator = copy.grids.SelectMany(item => item.Elements).GetEnumerator())
			using (var valueIterator = this.Values().GetEnumerator())
			{
				while (elementIterator.MoveNext() && valueIterator.MoveNext())
				{
					var value = valueIterator.Current;
					if (value.HasValue)
						elementIterator.Current.Value = value.Value;
				}
			}

			return copy;
		}
	}
}
