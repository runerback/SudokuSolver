using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public static class SudokuHelper
	{
		public static Definition.Sudoku Copy(this Definition.Sudoku source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			var target = new Definition.Sudoku();

			using (var sourceIterator = new SudokuElementEnumerable(source).GetEnumerator())
			using (var targetIterator = new SudokuElementEnumerable(target).GetEnumerator())
			{
				while (sourceIterator.MoveNext() & targetIterator.MoveNext())
				{
					var sourceElement = sourceIterator.Current;
					var targetElement = targetIterator.Current;

					if (sourceElement.HasValue)
						targetElement.SetValue(sourceElement.Value);
					else
						targetElement.ClearValue();
				}
			}

			return target;
		}

		public static void UpdateByValued(this Definition.Sudoku source, Definition.Sudoku other)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (other == null)
				throw new ArgumentNullException("other");

			using (var sourceIterator = new SudokuElementEnumerable(source).GetEnumerator())
			using (var otherIterator = new SudokuElementEnumerable(other).GetEnumerator())
			{
				while (sourceIterator.MoveNext() & otherIterator.MoveNext())
				{
					var sourceElement = sourceIterator.Current;
					var otherElement = otherIterator.Current;

					if (!sourceElement.HasValue && otherElement.HasValue)
						sourceElement.SetValue(otherElement.Value);
				}
			}
		}
	}
}
