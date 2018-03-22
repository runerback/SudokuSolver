using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Observers
{
	public class LineObserver
	{
		public LineObserver(Definition.Line line, int index)
		{
			if (line == null)
				throw new ArgumentNullException("line");
			if (index < Observer.MIN_INDEX_IN_NINE || index > Observer.MAX_INDEX_IN_NINE)
				throw new ArgumentOutOfRangeException("index");

			this.line = line;
			this.index = index;

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

		private Definition.Line line;

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var line = this.line;

			int emptyElementCount = line.Elements.Count(item => !item.HasValue);
			if (emptyElementCount <= 1)
			{
				if (emptyElementCount == 0)
					UntraceCompletedLine(line);

				if (Updated != null)
					Updated(this, new LineUpdatedEventArgs(line, this.index));
			}
		}

		private void UntraceCompletedLine(Definition.Line line)
		{
			if (line.Elements.All(item => item.HasValue))
			{
				foreach (var element in line.Elements)
				{
					element.ValueChanged -= onElementValueChanged;
				}
				this.isIdel = true;
			}
		}

		public event EventHandler<LineUpdatedEventArgs> Updated;

		private bool isIdel = false;
		public bool IsIdle
		{
			get { return this.isIdel; }
		}
	}
}
