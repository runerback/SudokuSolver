using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Resolve.Branch
{
	internal sealed class SudokuSnapshot
	{
		public SudokuSnapshot(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.value = createSnapshot(sudoku);
		}

		private string value;
		public string Value
		{
			get { return this.value; }
		}

		private const char EMPTY_ELEMENT_PLACEHOLDER = ' ';
		//number 1 in char is 49
		private const int NUMBER_CHAR_SPACE = 48;

		private string createSnapshot(Definition.Sudoku sudoku)
		{
			StringBuilder builder = new StringBuilder(81);
			foreach (var element in new SudokuElementEnumerable(sudoku))
			{
				if (element.HasValue)
					builder.Append(element.Value);
				else
					builder.Append(EMPTY_ELEMENT_PLACEHOLDER);
			}
			return builder.ToString();
		}

		public Definition.Sudoku BuildSudoku()
		{
			Definition.Sudoku sudoku = new Definition.Sudoku();

			using (var charIterator = this.value.GetEnumerator())
			using (var elementIterator = new SudokuElementEnumerable(sudoku).GetEnumerator())
			{
				while (charIterator.MoveNext() && elementIterator.MoveNext())
				{
					char value = charIterator.Current;
					if (value != EMPTY_ELEMENT_PLACEHOLDER)
					{
						elementIterator.Current.SetValue(value - NUMBER_CHAR_SPACE);
					}
				}
			}

			return sudoku;
		}
	}
}
