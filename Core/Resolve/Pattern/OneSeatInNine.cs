using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	/// <summary>
	/// only one element not filled, in Grid or Line
	/// </summary>
	internal sealed class OneSeatInNine : SudokuSolverPartternBase
	{
		public OneSeatInNine(Definition.Sudoku sudoku)
			: base(sudoku)
		{ }

		#region Block observers

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			var gridObservers = sudoku.Grids
				.Select(item => new Observers.GridObserver(item, Observers.SeatMode.One))
				.Where(item => !item.IsIdle)
				.ToArray();
			foreach (var gridObserver in gridObservers)
			{
				gridObserver.Updated += onGridUpdated;
			}

			var lineObservers = sudoku.Rows.Concat(sudoku.Columns)
				.Select(item => new Observers.OneSeatLineObserver(item, Observers.SeatMode.One))
				.Where(item => !item.IsIdle)
				.ToArray();
			foreach (var rowObserver in lineObservers)
			{
				rowObserver.Updated += onLineUpdated;
			}

			return gridObservers
				.Cast<Observers.ObserverBase>()
				.Concat(lineObservers)
				.ToArray();
		}

		private void onGridUpdated(object sender, Observers.GridUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Grid.Elements))
				((Observers.GridObserver)sender).Updated -= onGridUpdated;
		}

		private void onLineUpdated(object sender, Observers.LineUpdatedEventArgs e)
		{
			if (fillOnlyOneElement(e.Line.Elements))
				((Observers.OneSeatLineObserver)sender).Updated -= onLineUpdated;
		}

		#endregion Block observers

		#region Fill

		public override void Fill()
		{
			foreach (var elements in new SudokuBlockEnumerable(sudoku))
			{
				fillOnlyOneElement(elements);
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
			emptyElement.SetValue(value);

			return true;
		}

		#endregion Fill

		
	}
}
