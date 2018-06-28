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

		public const int SUDOKU_SPLIT_LEN = 9;
		public static readonly string SUDOKU_EMPTY_ELEMENT_VALUE = "_";

		public static bool TryParse(string source, out Definition.Sudoku sudoku)
		{
			sudoku = null;

			if (string.IsNullOrEmpty(source))
				return false;

			int size = SUDOKU_SPLIT_LEN;

			string[] lines = source.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			if (lines.Length != size)
				return false;

			int[] elementValues = new int[size * size];
			int valueIndex = 0;

			foreach (var line in lines)
			{
				int[] values;
				if (!TryParse(line, out values))
					return false;
				Array.Copy(values, 0, elementValues, valueIndex++ * size, size);
			}

			sudoku = new Definition.Sudoku();
			using (var sudokuElementIterator = new SudokuElementEnumerable(sudoku, SudokuElementTravelType.Row).GetEnumerator())
			using (var valueIterator = elementValues.AsEnumerable().GetEnumerator())
			{
				while (sudokuElementIterator.MoveNext() && valueIterator.MoveNext())
				{
					var element = sudokuElementIterator.Current;
					int value = valueIterator.Current;
					if (value > 0)
						element.SetValue(value);
				}
			}

			return true;
		}

		public static bool TryParse(string rowData, out int[] values)
		{
			values = null;

			if (string.IsNullOrEmpty(rowData))
				return false;

			string[] elements = rowData.Split(' ');
			if (elements.Length != SUDOKU_SPLIT_LEN)
				return false;

			int index = 0;
			values = new int[9];
			foreach (var elementValue in elements
				.Select(item => item.Trim()))
			{
				if (elementValue != SUDOKU_EMPTY_ELEMENT_VALUE)
				{
					int value;
					if (!int.TryParse(elementValue, out value))
						return false;
					values[index] = value;
				}
				index++;
			}
			return true;
		}

		public static bool Validate(this Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			foreach (var elements in new SudokuBlockEnumerable(sudoku))
			{
				if (!Validate(elements))
					return false;
			}
			return true;
		}

		public static bool Validate(this IEnumerable<Definition.Element> elements)
		{
			if (elements == null)
				throw new ArgumentNullException("elements");

			int index = 0;
			foreach (var value in elements.Values()
				.OrderBy(item => item))
			{
				bool matched = false;
				while (index++ < 9)
				{
					if (value == index)
					{
						index = value;
						matched = true;
						break;
					}
				}
				if(!matched)
					return false;
			}
			return true;
		}

		public static bool ValueEquals(this Definition.Sudoku first, Definition.Sudoku second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");

			return new SudokuElementEnumerable(first).SequenceEqual(
				new SudokuElementEnumerable(second),
				new ElementValueComparer());
		}
	}
}
