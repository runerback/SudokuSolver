using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;

namespace SudokuFiller
{
	sealed class GridInputKeyNavigation : Behavior<GridPresenter>
	{
		protected override void OnAttached()
		{
			this.AssociatedObject.Dispatcher.BeginInvoke((Action)delegate
			{
				initialize(this.AssociatedObject);
			}, System.Windows.Threading.DispatcherPriority.Loaded);
		}

		private void initialize(GridPresenter target)
		{
			//append behaviors
			var elementBehaviors = new List<ElementInputNavigation>();
			foreach (var element in target.Children<ElementPresenter>())
			{
				var behavior = new ElementInputNavigation();
				Interaction.GetBehaviors(element).Add(behavior);
				elementBehaviors.Add(behavior);

				behavior.Selected += onElementInputerSelected;
			}
			this.elementNavigations = elementBehaviors.ToArray();
		}

		protected override void OnDetaching()
		{
			foreach (var behavior in this.elementNavigations)
				behavior.Selected -= onElementInputerSelected;
		}

		private ElementInputNavigation[] elementNavigations;

		//select from UI
		private void onElementInputerSelected(object sender, EventArgs e)
		{
			this.elementIndex = ((ElementPresenter)sender).Element.Index;
			if (Selected != null)
				Selected(this.AssociatedObject, EventArgs.Empty);
		}

		//element of this grid is selected from UI
		public event EventHandler Selected;

		private int elementIndex = 0;
		public int CurrentElementIndex
		{
			get { return elementIndex; }
		}

		/// <summary>
		/// check whether next element is within current grid.
		/// </summary>
		/// <remarks>from self</remarks>
		public bool CanNavigate(NavigateDirection direction)
		{
			var elementIndex = this.elementIndex;

			if (elementIndex == 4) 
				return true;

			switch (direction)
			{
				case NavigateDirection.Left:
					return elementIndex % 3 != 0;
				case NavigateDirection.Right:
					return elementIndex % 3 != 2;
				case NavigateDirection.Up:
					return elementIndex / 3 != 0;
				case NavigateDirection.Down:
					return elementIndex / 3 != 2;
				default: throw new NotImplementedException();
			}
		}

		/// <remarks>from self</remarks>
		public void Navigate(NavigateDirection direction)
		{
			if (!CanNavigate(direction))
				throw new InvalidOperationException("edge reached");

			//Console.WriteLine("Navigating to {0}", direction);
			//Console.WriteLine("Current element index: {0}", elementIndex);
			var nextElementIndex = getNextElementIndex(this.elementIndex, direction);
			//Console.WriteLine("Next element index: {0}", nextElementIndex);
			var elementNavigation = this.elementNavigations[nextElementIndex];
			elementNavigation.Navigate();
			this.elementIndex = nextElementIndex;
		}

		/// <remarks>from another grid</remarks>
		public void Navigate(NavigateDirection direction, int elementLineIndex)
		{
			//Console.WriteLine("Navigating to {0} with line index {1}", direction, elementLineIndex);
			int nextElementIndex;
			switch (direction)
			{
				case NavigateDirection.Left:
					nextElementIndex = elementLineIndex * 3 + 2;
					break;
				case NavigateDirection.Right:
					nextElementIndex = elementLineIndex * 3;
					break;
				case NavigateDirection.Up:
					nextElementIndex = 6 + elementLineIndex;
					break;
				case NavigateDirection.Down:
					nextElementIndex = elementLineIndex;
					break;
				default: throw new NotImplementedException();
			}

			//Console.WriteLine("Next element index: {0}", nextElementIndex);
			var elementNavigation = this.elementNavigations[nextElementIndex];
			elementNavigation.Navigate();
			this.elementIndex = nextElementIndex;
		}

		private const int MAX_ElementIndex = 8;
		private const int MIN_ElementIndex = 0;

		private int getNextElementIndex(int currentElementIndex, NavigateDirection direction)
		{
			switch (direction)
			{
				case NavigateDirection.Left:
					return currentElementIndex == MIN_ElementIndex ? MAX_ElementIndex : currentElementIndex - 1;
				case NavigateDirection.Right:
					return currentElementIndex == MAX_ElementIndex ? MIN_ElementIndex : currentElementIndex + 1;
				case NavigateDirection.Up:
					return currentElementIndex / 3 == 0 ? currentElementIndex + 6 : currentElementIndex - 3;
				case NavigateDirection.Down:
					return currentElementIndex / 3 == 2 ? currentElementIndex - 6 : currentElementIndex + 3;
				default: throw new NotImplementedException();
			}
		}

		/// <summary>
		/// navigated to another grid
		/// </summary>
		public void Leave()
		{
			this.elementIndex = 0;
			//Console.WriteLine("Edge reached");
		}
	}
}
