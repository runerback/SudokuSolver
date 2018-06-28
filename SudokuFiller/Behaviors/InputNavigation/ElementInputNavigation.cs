using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SudokuFiller
{
	sealed class ElementInputNavigation : Behavior<ElementPresenter>
	{
		protected override void OnAttached()
		{
			this.AssociatedObject.Dispatcher.BeginInvoke((Action)delegate
			{
				initialize(this.AssociatedObject);
			}, System.Windows.Threading.DispatcherPriority.Loaded);
		}

		private void initialize(ElementPresenter target)
		{
			var inputer = target.Children<TextBox>().First();
			inputer.GotKeyboardFocus += OnFocused;
			this.inputer = inputer;

			var element = target.Element;
			if (element.GridIndex == 0 && element.Index == 0)
				Navigate();
		}

		protected override void OnDetaching()
		{
			this.inputer.GotKeyboardFocus -= OnFocused;
		}

		private TextBox inputer;

		private void OnFocused(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			if (this.isNavigating)
			{
				this.isNavigating = false;
				return;
			}
			
			if (this.Selected != null)
				this.Selected(this.AssociatedObject, EventArgs.Empty);
			this.isNavigating = false;
		}

		private bool isNavigating = false;
		/// <summary>
		/// navigate to this element, focus the inputer
		/// </summary>
		public void Navigate()
		{
			if (!this.AssociatedObject.IsFocused)
			{
				this.isNavigating = true;
				this.inputer.Focus();
			}
		}

		//focused from UI by user
		public event EventHandler Selected;
	}
}
