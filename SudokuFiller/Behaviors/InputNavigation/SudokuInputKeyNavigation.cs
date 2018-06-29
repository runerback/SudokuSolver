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
			var keyBindingBehavior = new KeyBindingsBehavior();
			keyBindingBehavior.KeyBindings.AddRange(
				new KeyBinding[]
				{
					new KeyBinding(this.NavLeftCommand, new KeyGesture(Key.Left)),
					new KeyBinding(this.NavRightCommand, new KeyGesture(Key.Right)),
					new KeyBinding(this.NavRightCommand, new KeyGesture(Key.Tab)),
					new KeyBinding(this.NavUpCommand, new KeyGesture(Key.Up)),
					new KeyBinding(this.NavDownCommand, new KeyGesture(Key.Down))
				});
			Interaction.GetBehaviors(target).Add(keyBindingBehavior);

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

		protected override void OnDetaching()
		{
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

		private const int MAX_LineIndex = 2;
		private const int MIN_LineIndex = 0;

		public void Navigate(NavigateDirection direction)
		{
			var currentGridIndex = this.currentGridIndex;
			//Console.WriteLine("Current Grid Index: {0}", currentGridIndex);
			var currentGridNavigation = gridNavigations[currentGridIndex];
			//Console.WriteLine("Navigating to {0} test", direction, currentGridIndex);
			if (currentGridNavigation.CanNavigate(direction))
			{
				//Console.WriteLine("Passed");
				currentGridNavigation.Navigate(direction);
			}
			else
			{
				var currentElementIndex = currentGridNavigation.CurrentElementIndex;

				//Console.WriteLine("Failed");
				currentGridNavigation.Leave();

				var gridIndex = currentGridIndex;
				var elementLineIndex = getElementLineIndex(currentElementIndex, direction);

				//gridIndex -> nextGirdIndex, elementLineIndex -> nextElementLineIndex
				getNextPosition81(ref gridIndex, ref elementLineIndex, direction);

				var nextGridNavigation = gridNavigations[gridIndex];
				//Console.WriteLine("Navigating to grid {0}", gridIndex);
				nextGridNavigation.Navigate(direction, elementLineIndex);

				this.currentGridIndex = gridIndex;
			}
		}

		[Obsolete("use getNextPosition81 instead")]
		private int getNextGridIndex(int currentGridIndex, NavigateDirection direction)
		{
			switch (direction)
			{
				case NavigateDirection.Left:
					return currentGridIndex == MIN_GridIndex ? MAX_GridIndex : currentGridIndex - 1;
				case NavigateDirection.Right:
					return currentGridIndex == MAX_GridIndex ? MIN_GridIndex : currentGridIndex + 1;
				case NavigateDirection.Up:
					return currentGridIndex / 3 == MIN_LineIndex ? currentGridIndex + 6 : currentGridIndex - 3;
				case NavigateDirection.Down:
					return currentGridIndex / 3 == MAX_LineIndex ? currentGridIndex - 6 : currentGridIndex + 3;
				default: throw new NotImplementedException();
			}
		}

		private void getNextPosition81(ref int gridIndex, ref int elementLineIndex, NavigateDirection direction)
		{
			var currentGridIndex = gridIndex;
			var currentElementLineIndex = elementLineIndex;

			switch (direction)
			{
				case NavigateDirection.Left:
					{
						if (currentGridIndex % 3 == MIN_LineIndex)
						{
							if (currentElementLineIndex == MIN_LineIndex)
							{
								gridIndex = currentGridIndex == MIN_GridIndex ? MAX_GridIndex : currentGridIndex - 1;
								elementLineIndex = MAX_LineIndex;
							}
							else
							{
								gridIndex = currentGridIndex + MAX_LineIndex;
								elementLineIndex = currentElementLineIndex - 1;
							}
						}
						else
						{
							gridIndex = currentGridIndex - 1;
						}
					}
					break;
				case NavigateDirection.Right:
					{
						if (currentGridIndex % 3 == MAX_LineIndex)
						{
							if (currentElementLineIndex == MAX_LineIndex)
							{
								gridIndex = currentGridIndex == MAX_GridIndex ? MIN_GridIndex : currentGridIndex + 1;
								elementLineIndex = MIN_LineIndex;
							}
							else
							{
								gridIndex = currentGridIndex - MAX_LineIndex;
								elementLineIndex = currentElementLineIndex + 1;
							}
						}
						else
						{
							gridIndex = currentGridIndex + 1;
						}
					}
					break;
				case NavigateDirection.Up:
					{
						gridIndex = currentGridIndex / 3 == MIN_LineIndex ? currentGridIndex + 6 : currentGridIndex - 3;
					}
					break;
				case NavigateDirection.Down:
					{
						gridIndex = currentGridIndex / 3 == MAX_LineIndex ? currentGridIndex - 6 : currentGridIndex + 3;
					}
					break;
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
