using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Business
{
	internal sealed class SudokuStepSync : SudokuValueSyncBase
	{
		public SudokuStepSync(Definition.Sudoku defSudoku, Model.Sudoku sudoku)
			: base(defSudoku, sudoku)
		{ }

		protected override void onElementValueChanged(Model.Element element, bool hasValue, int newValue)
		{
			if (NewStep != null)
				NewStep(this, new SudokuNewStepEventArgs(
					new SudokuSolveStep(element, hasValue ? newValue : default(int?))));
		}

		public event EventHandler<SudokuNewStepEventArgs> NewStep;
	}

	internal sealed class SudokuNewStepEventArgs : EventArgs
	{
		public SudokuNewStepEventArgs(SudokuSolveStep step)
		{
			if (step == null)
				throw new ArgumentNullException("step");
			this.step = step;
		}

		private SudokuSolveStep step;
		public SudokuSolveStep Step
		{
			get { return this.step; }
		}
	}
}
