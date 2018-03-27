using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	public class GridLineObserver
	{
		public GridLineObserver(Definition.GridLine line, int index, GridLineObserverMode mode)
		{
			if (line == null)
				throw new ArgumentNullException("line");
			if (index < Observer.MIN_INDEX_IN_GRID_LINE || 
				index > Observer.MAX_INDEX_IN_GRID_LINE)
				throw new ArgumentOutOfRangeException("index");

			this.line = line;
			this.index = index;
			this.mode = mode;

			switch (mode)
			{
				case GridLineObserverMode.OneSeat:
					this.remaindSeatCount = 1;
					break;
				case GridLineObserverMode.AllSeat:
					this.remaindSeatCount = 3;
					break;
				default:
					throw new NotImplementedException(mode.ToString());
			}

			var uncompletedElements = line.Elements.Where(item => !item.HasValue);

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

		private Definition.GridLine line;

		private GridLineObserverMode mode;
		public GridLineObserverMode Mode
		{
			get { return this.mode; }
		}

		private int remaindSeatCount;

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var line = this.line;

			int emptyElementCount = line.Elements.Count(item => !item.HasValue);
			if (emptyElementCount <= this.remaindSeatCount)
			{
				if (emptyElementCount == 0)
					UntraceCompletedLine(line);

				if (Updated != null)
					Updated(this, new GridLineUpdatedEventArgs(line, this.index));
			}
		}

		private void UntraceCompletedLine(Definition.GridLine line)
		{
			if (line.Elements.All(item => item.HasValue))
			{
				foreach (var element in line.Elements)
				{
					element.ValueChanged -= onElementValueChanged;
				}
			}
		}

		public event EventHandler<GridLineUpdatedEventArgs> Updated;

		private bool isIdel = false;
		public bool IsIdle
		{
			get { return this.isIdel; }
		}
	}
}
