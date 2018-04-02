using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal sealed class TwoSeatsInLine : SudokuSolverPartternBase
	{
		public TwoSeatsInLine(Definition.Sudoku sudoku)
			: base(sudoku)
		{
		}

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{

			foreach (var lineObserver in sudoku.Rows.Concat(sudoku.Columns)
				.Select(item => new Observers.LineObserver(item, Observers.SeatMode.Two)))
			{
				if (lineObserver.IsIdle)
				{
					lineObserver.Dispose();
				}
				else
				{
					lineObserver.Updated += onLineUpdated;
					yield return lineObserver;
				}
			}
		}

		private void onLineUpdated(object sender, Observers.LineUpdatedEventArgs e)
		{
			if (fillOnlyTwoElement(e.Line))
				((Observers.LineObserver)sender).Updated -= onLineUpdated;
		}

		private bool fillOnlyTwoElement(Definition.Line line)
		{
			var twoEmptyElementsQuery = line.Elements.OnlyTwo(item => !item.HasValue);
			if (!twoEmptyElementsQuery.Any())
				return false;

			Definition.Element emptyElement1, emptyElement2;
			using (var iterator = twoEmptyElementsQuery.GetEnumerator())
			{
				iterator.MoveNext();
				emptyElement1 = iterator.Current;
				iterator.MoveNext();
				emptyElement2 = iterator.Current;
			}

			var valuesRemainder = line.Elements
				.Values()
				.SudokuExcept()
				.ToArray();

			var sudoku = this.sudoku;

			int emptyElement1Index = emptyElement1.Index;
			int emptyElement2Index = emptyElement2.Index;

			int sourceGrid1Index = emptyElement1.GridIndex;
			int sourceGrid2Index = emptyElement2.GridIndex;

			bool inSameGrid = sourceGrid1Index == sourceGrid2Index;

			var sourceGrid1 = sudoku.Grids[sourceGrid1Index];
			var sourceGrid2 = inSameGrid ? sourceGrid1 : sudoku.Grids[sourceGrid2Index];

			var lineType = getOppositeLineType(line.LineType);

			Definition.Grid otherGrid1ForElement1, otherGrid2ForElement1;
			sourceGrid1.GetOtherGrids(lineType, out otherGrid1ForElement1, out otherGrid2ForElement1);

			Definition.Grid otherGrid1ForElement2, otherGrid2ForElement2;
			if (inSameGrid)
			{
				otherGrid1ForElement2 = otherGrid1ForElement1;
				otherGrid2ForElement2 = otherGrid2ForElement1;
			}
			else
			{
				sourceGrid2.GetOtherGrids(lineType, out otherGrid1ForElement2, out otherGrid2ForElement2);
			}

			int layer1, layer2;
			switch (lineType)
			{
				case Definition.LineType.Row:
					layer1 = emptyElement1Index / 3;
					layer2 = emptyElement2Index / 3;
					break;
				case Definition.LineType.Column:
					layer1 = emptyElement1Index % 3;
					layer2 = emptyElement2Index % 3;
					break;
				default: throw new NotImplementedException();
			}

			var valuesForEmptyElement1 =
				otherGrid1ForElement1.GetElementsInCurrentGridLine(layer1, lineType)
				.Concat(otherGrid2ForElement1.GetElementsInCurrentGridLine(layer1, lineType))
				.Values();

			var except1 = valuesRemainder.Except(valuesForEmptyElement1);
			int value1 = except1.SingleOrDefault(-1);
			if (value1 > 0)
			{
				emptyElement1.SetValue(value1);
				emptyElement2.SetValue(valuesRemainder.Except(value1).First());

				return true;
			}

			var valuesForEmptyElement2 =
				otherGrid1ForElement2.GetElementsInCurrentGridLine(layer2, lineType)
				.Concat(otherGrid2ForElement2.GetElementsInCurrentGridLine(layer2, lineType))
				.Values();

			var except2 = valuesRemainder.Except(valuesForEmptyElement2);
			int value2 = except2.SingleOrDefault(-1);
			if (value2 > 0)
			{
				emptyElement2.SetValue(value2);
				emptyElement1.SetValue(valuesRemainder.Except(value2).First());

				return true;
			}

			return false;
		}

		private Definition.LineType getOppositeLineType(Definition.LineType sourceLineType)
		{
			switch (sourceLineType)
			{
				case Definition.LineType.Row:
					return Definition.LineType.Column;
				case Definition.LineType.Column:
					return Definition.LineType.Row;
				default: throw new NotImplementedException();
			}
		}

		public override void Fill()
		{
			foreach (var line in new LineEnumerable(this.sudoku, Definition.LineType.Row)
				.Concat(new LineEnumerable(this.sudoku, Definition.LineType.Column)))
			{
				fillOnlyTwoElement(line);
			}
		}

		private const int index = 3;
		public override int Index
		{
			get { return index; }
		}
	}
}
