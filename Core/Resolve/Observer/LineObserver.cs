using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal class OneSeatLineObserver
	{
		public OneSeatLineObserver(Definition.Line line, SeatMode seatMode)
		{
			if (line == null)
				throw new ArgumentNullException("line");

			this.line = line;
			this.seatMode = seatMode;

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

		private Definition.Line line;

		private SeatMode seatMode;
		public SeatMode SeatMode
		{
			get { return seatMode; }
		}

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var line = this.line;

			int emptyElementCount = line.Elements.Count(item => !item.HasValue);
			if (emptyElementCount <= 1)
			{
				if (emptyElementCount == 0)
					UntraceCompletedLine(line);

				if (Updated != null)
					Updated(this, new LineUpdatedEventArgs(line));
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
