using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Observers
{
	public class GridObserver
	{
		public GridObserver(Definition.Grid grid, int index)
		{
			if (grid == null)
				throw new ArgumentNullException("grid");
			if (index < Observer.MIN_INDEX_IN_NINE || index > Observer.MAX_INDEX_IN_NINE)
				throw new ArgumentOutOfRangeException("index");

			this.grid = grid;
			this.index = index;

			var uncompletedElements = grid.Elements.Where(item => !item.HasValue);

			if (!uncompletedElements.Any())
			{
				this.isIdel = true;
				return;
			}

			foreach (var element in uncompletedElements)
			{
				element.ValueChanged += onElementValueChanged;
			}
		}	

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		private Definition.Grid grid;

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var grid = this.grid;

			int emptyElementCount = grid.Elements.Count(item => !item.HasValue);
			if (emptyElementCount <= 1)
			{
				if (emptyElementCount == 0)
					UntraceCompletedGrid(grid);

				if (Updated != null)
					Updated(this, new GridUpdatedEventArgs(grid, this.index));
			}
		}

		private void UntraceCompletedGrid(Definition.Grid grid)
		{
			if (grid.Elements.All(item => item.HasValue))
			{
				foreach (var element in grid.Elements)
				{
					element.ValueChanged -= onElementValueChanged;
				}
				this.isIdel = true;
			}
		}

		public event EventHandler<GridUpdatedEventArgs> Updated;

		private bool isIdel = false;
		public bool IsIdle
		{
			get { return this.isIdel; }
		}
	}
}
