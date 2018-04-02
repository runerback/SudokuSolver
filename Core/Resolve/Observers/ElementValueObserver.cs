using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal abstract class ElementValueObserver : ObserverBase
	{
		public ElementValueObserver(Definition.IElementCluster elementCluster, SeatMode seatMode)
		{
			if (elementCluster == null)
				throw new ArgumentNullException("elementCluster");
			this.elementCluster = elementCluster;
			this.seatMode = seatMode;
			this.observingSeatsCount = (int)seatMode;

			var uncompletedElements = elementCluster.Elements.Where(item => !item.HasValue);

			if (!uncompletedElements.Any())
			{
				this.isIdel = true;
			}
			else
			{
				foreach (var element in uncompletedElements)
				{
					element.ValueChanged += onElementValueChanged;
				}
			}
		}

		private readonly Definition.IElementCluster elementCluster;
		public Definition.IElementCluster ElementCluster
		{
			get { return elementCluster; }
		}

		private void onElementValueChanged(object sender, EventArgs e)
		{
			var elements = this.elementCluster.Elements;

			int emptyElementsCount = elements.Count(item => !item.HasValue);
			if (emptyElementsCount == 0)
				UntraceCompletedElements(elements);

			onElementClusterValueChanged(
				sender,
				emptyElementsCount);
		}

		protected abstract void onElementClusterValueChanged(object elementObj, int emptyElementsCount);

		private SeatMode seatMode;
		public SeatMode SeatMode
		{
			get { return this.seatMode; }
		}

		protected int observingSeatsCount;

		private bool isIdel = false;
		public bool IsIdle
		{
			get { return this.isIdel; }
		}

		private void UntraceCompletedElements(IEnumerable<Definition.Element> elements)
		{
			if (!this.isIdel)
			{
				bool allUntraced = true;
				foreach (var element in elements)
				{
					if (element.HasValue)
						element.ValueChanged -= onElementValueChanged;
					else
						allUntraced = false;
				}
				this.isIdel = allUntraced;
			}
		}

		protected override void Disposing()
		{
			foreach (var element in this.elementCluster.Elements)
			{
				element.ValueChanged -= onElementValueChanged;
			}
		}
	}
}
