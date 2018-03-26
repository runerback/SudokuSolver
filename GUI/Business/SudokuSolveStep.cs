using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Business
{
	internal class SudokuSolveStep
	{
		public SudokuSolveStep(Definition.Element element)
		{
			this.element = element;
			this.originValue = element.Value;
		}

		public SudokuSolveStep(Definition.Element element, int newValue)
			: this(element)
		{
			this.newValue = newValue;
		}

		private Definition.Element element;
		private int? newValue = null;
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
			if (value.HasValue)
				this.element.SetValue(value.Value);
			else
				this.element.ClearValue();
		}
	}
}
