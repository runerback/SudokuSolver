using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuFiller
{
	sealed class SudokuDuplicatedValueHelper
	{
		private const int SUDOKU_MAXVALUE = 9;

		/// <returns>if find any duplicated element then return the index of first item, otherwise return -1</returns>
		public static int GetDuplicatedValueIndex(IEnumerable<int?> values)
		{
			if (values == null || !values.Any())
				throw new ArgumentNullException("values");

			int expectedValue = 0;
			int previousIndex = -1;

			foreach (var current in values
				.Select((value, index) => new IndexedValue(value, index))
				.Where(item => item.HasValue)
				.OrderBy(item => item.Value))
			{
				int value = current.Value;
				int index = current.Index;

				bool matched = false;
				while (expectedValue++ < SUDOKU_MAXVALUE)
				{
					if (value == expectedValue)
					{
						expectedValue = value;
						matched = true;
						break;
					}
				}

				if (!matched)
					return previousIndex;

				previousIndex = index;
			}
			return -1;
		}

		class IndexedValue
		{
			public IndexedValue(int? value, int index)
			{
				this.index = index;
				if (value.HasValue)
				{
					this.hasValue = true;
					this.value = value.Value;
				}
			}

			private readonly int index;
			public int Index
			{
				get { return this.index; }
			}

			private readonly bool hasValue;
			public bool HasValue
			{
				get { return this.hasValue; }
			}

			private readonly int value = 0;
			public int Value
			{
				get { return this.value; }
			}
		}
	}
}
