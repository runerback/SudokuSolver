using Runerback.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace SudokuFiller
{
	sealed class SudokuInputKeyNavigation : Behavior<SudokuPresenter>
	{
		protected override void OnAttached()
		{
			this.AssociatedObject.Dispatcher.BeginInvoke((Action)delegate
			{
				initialize(this.AssociatedObject);
			}, System.Windows.Threading.DispatcherPriority.Loaded);
		}

		private void initialize(SudokuPresenter target)
		{
			var window = target.ContainerWindow();
			///TODO: textbox swallowed key event
			window.InputBindings.AddRange(
				new InputBinding[]
				{
					new InputBinding(this.NavLeftCommand, new KeyGesture(Key.Left)),
					new InputBinding(this.NavRightCommand, new KeyGesture(Key.Right)),
					new InputBinding(this.NavRightCommand, new KeyGesture(Key.Tab)),
					new InputBinding(this.NavUpCommand, new KeyGesture(Key.Up)),
					new InputBinding(this.NavDownCommand, new KeyGesture(Key.Down))
				});
			this.inputBindingTarget = window;

			//append behaviors
			var gridBehaviors = new List<GridInputKeyNavigation>();
			foreach (var grid in target.Children<GridPresenter>())
			{
				var behavior = new GridInputKeyNavigation();
				Interaction.GetBehaviors(grid).Add(behavior);
				gridBehaviors.Add(behavior);
				behavior.Selected += onGridSelected;
			}
			this.gridNavigations = gridBehaviors.ToArray();
		}

		private System.Windows.Window inputBindingTarget;

		protected override void OnDetaching()
		{
			this.inputBindingTarget.InputBindings.Clear();
			foreach (var behavior in this.gridNavigations)
				behavior.Selected -= onGridSelected;
		}

		//element of this grid is selected from UI
		private void onGridSelected(object sender, EventArgs e)
		{
			this.currentGridIndex = ((GridPresenter)sender).Grid.Index;
		}

		private GridInputKeyNavigation[] gridNavigations;

		//navigate start from (0, 0)
		private int currentGridIndex = 0;

		private const int MAX_GridIndex = 8;
		private const int MIN_GridIndex = 0;

		public void Navigate(NavigateDirection direction)
		{
			var currentGridNavigation = gridNavigations[currentGridIndex];
			if (currentGridNavigation.CanNavigate(direction))
			{
				currentGridNavigation.Navigate(direction);
			}
			else
			{
				currentGridNavigation.Leave();

				var nextGridIndex = getNextGridIndex(currentGridIndex, direction);
				var nextGridNavigation = gridNavigations[currentGridIndex];

				var elementLineIndex = getElementLineIndex(currentGridNavigation.CurrentElementIndex, direction);
				nextGridNavigation.Navigate(direction, elementLineIndex);

				this.currentGridIndex = nextGridIndex;
			}
		}

		private int getNextGridIndex(int currentGridIndex, NavigateDirection direction)
		{
			switch (direction)
			{
				case NavigateDirection.Left:
					return currentGridIndex == MIN_GridIndex ? MAX_GridIndex : currentGridIndex - 1;
				case NavigateDirection.Right:
					return currentGridIndex == MAX_GridIndex ? MIN_GridIndex : currentGridIndex + 1;
				case NavigateDirection.Up:
					return currentGridIndex / 3 == 0 ? currentGridIndex + 6 : currentGridIndex - 3;
				case NavigateDirection.Down:
					return currentGridIndex / 3 == 2 ? currentGridIndex - 6 : currentGridIndex + 3;
				default: throw new NotImplementedException();
			}
		}

		private int getElementLineIndex(int elementIndex, NavigateDirection direction)
		{
			switch (direction)
			{
				case NavigateDirection.Left:
				case NavigateDirection.Right:
					return elementIndex / 3;
				case NavigateDirection.Up:
				case NavigateDirection.Down:
					return elementIndex % 3;
				default: throw new NotImplementedException();
			}
		}
		
		#region Commands

		public SudokuInputKeyNavigation()
		{
			this.NavLeftCommand = new SimpleCommand(navLeft);
			this.NavRightCommand = new SimpleCommand(navRight);
			this.NavUpCommand = new SimpleCommand(navUp);
			this.NavDownCommand = new SimpleCommand(navDown);
		}

		private readonly ICommand NavLeftCommand;
		private readonly ICommand NavRightCommand;
		private readonly ICommand NavUpCommand;
		private readonly ICommand NavDownCommand;

		private void navLeft(object obj)
		{
			Navigate(NavigateDirection.Left);
		}

		private void navRight(object obj)
		{
			Navigate(NavigateDirection.Right);
		}

		private void navUp(object obj)
		{
			Navigate(NavigateDirection.Up);
		}

		private void navDown(object obj)
		{
			Navigate(NavigateDirection.Down);
		}

		#endregion Commands

	}
}
