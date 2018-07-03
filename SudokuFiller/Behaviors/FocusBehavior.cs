using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SudokuFiller
{
	sealed class FocusBehavior : Behavior<FrameworkElement>
	{
		public FocusBehavior()
		{

		}

		public bool IsFocused
		{
			get { return (bool)this.GetValue(IsFocusedProperty); }
			set { this.SetValue(IsFocusedProperty, value); }
		}

		public static readonly DependencyProperty IsFocusedProperty =
			DependencyProperty.Register(
				"IsFocused",
				typeof(bool),
				typeof(FocusBehavior),
				new PropertyMetadata(OnIsFocusedPropertyChanged));

		private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var target = ((FocusBehavior)d).AssociatedObject;

			if ((bool)e.NewValue)
				target.Focus();
			else
				target.MoveFocus(new TraversalRequest(FocusNavigationDirection.Last));
		}
		
		protected override void OnAttached()
		{
			var target = this.AssociatedObject;
			target.Dispatcher.BeginInvoke((Action)delegate
			{
				initialize(target);
			}, System.Windows.Threading.DispatcherPriority.Loaded);
		}

		private void initialize(FrameworkElement target)
		{
			var focusBinding = BindingOperations.GetBinding(this, IsFocusedProperty);
			if (focusBinding != null)
			{
				BindingOperations.SetBinding(
					this,
					IsFocusedProperty,
					focusBinding.UpdateSource(target.DataContext));
			}
		}
	}
}
