using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
	sealed class SudokuInputValidator : SudokuInputValidatorTestInterface, IDisposable
	{
		public SudokuInputValidator(SudokuData sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.sudoku = sudoku;

			foreach (var grid in sudoku.Grids)
				foreach (INotifyPropertyChanged target in grid.Elements)
					target.PropertyChanged += onElementValueChanged;
		}

		private readonly SudokuData sudoku;

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var element = (ElementData)sender;

			string rowValidateResult, columnValidateResult, gridValidateResult;
			var errorBuilder = new StringBuilder();

			if (!validateRow(element, out rowValidateResult))
				errorBuilder.AppendLine(rowValidateResult);

			if (!validateColumn(element, out columnValidateResult))
				errorBuilder.AppendLine(rowValidateResult);

			if (!validateGrid(element, out gridValidateResult))
				errorBuilder.AppendLine(rowValidateResult);

			element.SetError(errorBuilder.ToString());
		}

		private bool validateRow(ElementData element, out string errorInfo)
		{
			int gridIndex = element.GridIndex;
			int elementIndex = element.Index;
			var elements = getElementsInRow(gridIndex, elementIndex);
			var duplicatedElementIndex = getDuplicatedElementIndex(elements.Select(item => item.Value));
			if (duplicatedElementIndex > 0)
			{
				errorInfo = string.Format("value {0} already exists in current row at {1}",
					element.Value,
					duplicatedElementIndex + 1);
				return false;
			}

			errorInfo = null;
			return true;
		}

		private bool validateColumn(ElementData element, out string errorInfo)
		{
			int gridIndex = element.GridIndex;
			int elementIndex = element.Index;
			var elements = getElementsInColumn(gridIndex, elementIndex);
			var duplicatedElementIndex = getDuplicatedElementIndex(elements.Select(item => item.Value));
			if (duplicatedElementIndex > 0)
			{
				errorInfo = string.Format("value {0} already exists in current column at {1}",
					element.Value,
					duplicatedElementIndex + 1);
				return false;
			}

			errorInfo = null;
			return true;
		}

		private bool validateGrid(ElementData element, out string errorInfo)
		{
			int gridIndex = element.GridIndex;
			var elements = getElementsInGrid(gridIndex);
			var duplicatedElementIndex = getDuplicatedElementIndex(elements.Select(item => item.Value));
			if (duplicatedElementIndex > 0)
			{
				errorInfo = string.Format("value {0} already exists in current grid at {1}",
					element.Value,
					duplicatedElementIndex + 1);
				return false;
			}

			errorInfo = null;
			return true;
		}

		private const int GRID_SIZE = 3;

		private IEnumerable<ElementData> getElementsInRow(int gridIndex, int elementIndex)
		{
			var grids = this.sudoku.Grids;

			var gridRowIndex = gridIndex / GRID_SIZE;
			var elementRowIndex = elementIndex / GRID_SIZE;

			for (int gridOffset = 0; gridOffset < GRID_SIZE; gridOffset++)
				for (int elementOffset = 0; elementOffset < GRID_SIZE; elementOffset++)
					yield return grids[gridRowIndex * GRID_SIZE + gridOffset]
						.Elements[elementRowIndex * GRID_SIZE + elementOffset];
		}

		private IEnumerable<ElementData> getElementsInColumn(int gridIndex, int elementIndex)
		{
			var grids = this.sudoku.Grids;

			var gridColumnIndex = gridIndex % GRID_SIZE;
			var elementColumnIndex = elementIndex % GRID_SIZE;

			for (int gridOffset = 0; gridOffset < GRID_SIZE; gridOffset++)
				for (int elementOffset = 0; elementOffset < GRID_SIZE; elementOffset++)
					yield return grids[gridColumnIndex + gridOffset * GRID_SIZE]
						.Elements[elementColumnIndex + elementOffset * GRID_SIZE];
		}

		private IEnumerable<ElementData> getElementsInGrid(int gridIndex)
		{
			foreach (var element in this.sudoku.Grids[gridIndex].Elements)
				yield return element;
		}

		/// <returns>if find any duplicated element then return the index, otherwise return 0 (first element must not be duplicated)</returns>
		private int getDuplicatedElementIndex(IEnumerable<int?> values)
		{
			int index = 0;
			int expectedValue = 0;

			foreach (var current in values
				.OrderBy(item => item))
			{
				if (!current.HasValue)
				{
					index++;
					continue;
				}

				int value = current.Value;

				bool matched = false;
				while (expectedValue++ < 9)
				{
					if (current == expectedValue)
					{
						expectedValue = value;
						matched = true;
						break;
					}
				}

				if (!matched)
					return index;

				index++;
			}
			return 0;
		}

		#region SudokuInputValidatorTestInterface

		int SudokuInputValidatorTestInterface.GetDuplicatedElementIndex(IEnumerable<int?> values)
		{
			return getDuplicatedElementIndex(values);
		}

		IEnumerable<Tuple<int, int>> SudokuInputValidatorTestInterface.GetElementsInRow(int gridIndex, int elementIndex)
		{
			return getElementsInRow(gridIndex, elementIndex)
				.Select(element => Tuple.Create(element.GridIndex, element.Index));
		}

		IEnumerable<Tuple<int, int>> SudokuInputValidatorTestInterface.GetElementsInColumn(int gridIndex, int elementIndex)
		{
			return getElementsInColumn(gridIndex, elementIndex)
				.Select(element => Tuple.Create(element.GridIndex, element.Index));
		}

		#endregion SudokuInputValidatorTestInterface

		public void Dispose()
		{
			foreach (var grid in this.sudoku.Grids)
				foreach (INotifyPropertyChanged target in grid.Elements)
					target.PropertyChanged -= onElementValueChanged;
		}
	}
}
