using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Business
{
	internal sealed class SudokuAutoSync : SudokuValueSyncBase
	{
		public SudokuAutoSync(Definition.Sudoku defSudoku, Model.Sudoku sudoku)
			: base(defSudoku, sudoku)
		{ }

		protected override void onElementValueChanged(Model.Element element, bool hasValue, int newValue)
		{
			element.Value = hasValue ? newValue : default(int?);
		}
	}
}
