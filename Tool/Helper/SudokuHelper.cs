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

		private const int SUDOKU_SPLIT_LEN = 9;
		private static readonly string SUDOKU_EMPTY_ELEMENT_VALUE = "_";

		public static bool TryParse(string source, out Definition.Sudoku sudoku)
		{
			sudoku = null;

			if (string.IsNullOrEmpty(source))
				return false;
			string[] lines = source.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			if (lines.Length != SUDOKU_SPLIT_LEN)
				return false;

			int[] elementValues = new int[SUDOKU_SPLIT_LEN * SUDOKU_SPLIT_LEN];
			int valueIndex = 0;

			foreach (var line in lines)
			{
				string[] elements = line.Split(' ');
				if (elements.Length != SUDOKU_SPLIT_LEN)
					return false;
				foreach (var elementValue in elements
					.Select(item => item.Trim()))
				{
					if (elementValue != SUDOKU_EMPTY_ELEMENT_VALUE)
					{
						int value;
						if (!int.TryParse(elementValue, out value))
							return false;
						elementValues[valueIndex] = value;
					}
					valueIndex++;
				}
			}

			sudoku = new Definition.Sudoku();
			using (var sudokuElementIterator = new SudokuElementEnumerable(sudoku, SudokuElementTravelType.Row).GetEnumerator())
			using (var valueIterator = elementValues.AsEnumerable().GetEnumerator())
			{
				while (sudokuElementIterator.MoveNext() && valueIterator.MoveNext())
				{
					int value = valueIterator.Current;
					if (value > 0)
					{
						sudokuElementIterator.Current.SetValue(value);
					}
				}
			}

			return true;
		}

		public static bool Valdiate(this Definition.Sudoku sudoku)
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

			int value = 0;
			foreach (int element in elements
				.Where(element => element.HasValue)
				.OrderBy(element => element))
			{
				bool matched = false;
				while (value++ < 9)
				{
					if (element == value)
					{
						value = element;
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
