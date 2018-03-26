using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SudokuSolver.GUI
{
	public class WindowDragGizmo : ContentControl
	{
		public Window Window
		{
			get { return (Window)this.GetValue(WindowProperty); }
			set { this.SetValue(WindowProperty, value); }
		}

		public static readonly DependencyProperty WindowProperty =
			DependencyProperty.Register(
				"Window",
				typeof(Window),
				typeof(WindowDragGizmo));

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);

			if (Window != null)
				Window.DragMove();
		}
	}
}
