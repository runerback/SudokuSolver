using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Business
{
	internal class SudokuSolveStep
	{
		public SudokuSolveStep(Model.Element element, int? newValue)
		{
			this.element = element;
			this.originValue = element.Value;
			this.newValue = newValue;
		}

		private Model.Element element;
		private int? newValue;
		private int? originValue;

		public void Execute()
		{
			setValue(newValue);
		}

		public void Restore()
		{
			setValue(originValue);
		}

		private void setValue(int? value)
		{
			this.element.Value = value;
		}
	}
}
