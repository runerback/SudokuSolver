using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	/// <summary>
	/// 0. only one element not filled, in Grid or sudoku Line
	/// </summary>
	internal sealed class OneSeatInNine : SudokuSolverPartternBase
	{
		public OneSeatInNine(Definition.Sudoku sudoku)
			: base(sudoku)
		{ }

		#region Block observers

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			foreach (var gridObserver in sudoku.Grids
				.Select(item => new Observers.GridObserver(item, Observers.SeatMode.One)))
			{
				if (gridObserver.IsIdle)
				{
					gridObserver.Dispose();
				}
				else
				{
					gridObserver.Updated += onGridUpdated;
					yield return gridObserver;
				}
			}

			foreach (var lineObserver in sudoku.Rows.Concat(sudoku.Columns)
				.Select(item => new Observers.LineObserver(item, Observers.SeatMode.One)))
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

		private void onGridUpdated(object sender, Observers.GridUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Grid.Elements))
				((Observers.GridObserver)sender).Updated -= onGridUpdated;
		}

		private void onLineUpdated(object sender, Observers.LineUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Line.Elements))
				((Observers.LineObserver)sender).Updated -= onLineUpdated;
		}

		#endregion Block observers

		#region Fill

		public override void Fill()
		{
			foreach (var elements in new SudokuBlockEnumerable(sudoku))
			{
				if (fillOnlyOneElement(elements) && HasFailed)
					break;
			}
		}

		private bool fillOnlyOneElement(IEnumerable<Definition.Element> elements)
		{
			if (elements == null)
				throw new ArgumentNullException("elements");

			//get empty element
			Definition.Element emptyElement = null;
			using (var e = elements
				.Where(item => !item.HasValue)
				.GetEnumerator())
			{
				if (!e.MoveNext()) return true; //not empty element
				emptyElement = e.Current;
				if (e.MoveNext()) return false; //more than one empty element
			}

			int value = 1;
			foreach (int element in elements
				.Where(item => item.HasValue)
				.OrderBy(item => item))
			{
				if (element != value)
					break;
				value++;
			}
			filling(emptyElement, value);
			emptyElement.SetValue(value);

			return true;
		}

		#endregion Fill

		private const int index = 0;
		public override int Index
		{
			get { return index; }
		}
	}
}
