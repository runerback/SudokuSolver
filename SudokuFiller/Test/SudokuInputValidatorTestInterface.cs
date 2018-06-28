using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuFiller
{
	public interface SudokuInputValidatorTestInterface
	{
		int GetDuplicatedElementIndex(IEnumerable<int?> values);
		IEnumerable<Tuple<int, int>> GetElementsInRow(int gridIndex, int elementIndex);
		IEnumerable<Tuple<int, int>> GetElementsInColumn(int gridIndex, int elementIndex);
	}
}
