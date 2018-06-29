using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SudokuFiller
{
	sealed class ControlPanel : Control
	{
		public ControlPanel()
		{

		}
		
		#region FillCommand

		public ICommand FillCommand
		{
			get { return (ICommand)this.GetValue(FillCommandProperty); }
			set { this.SetValue(FillCommandProperty, value); }
		}

		public static readonly DependencyProperty FillCommandProperty =
			DependencyProperty.Register(
				"FillCommand",
				typeof(ICommand),
				typeof(ControlPanel));
		
		#endregion FillCommand

		#region ResetCommand

		public ICommand ResetCommand
		{
			get { return (ICommand)this.GetValue(ResetCommandProperty); }
			set { this.SetValue(ResetCommandProperty, value); }
		}

		public static readonly DependencyProperty ResetCommandProperty =
			DependencyProperty.Register(
				"ResetCommand",
				typeof(ICommand),
				typeof(ControlPanel));
		
		#endregion ResetCommand

		#region ShowSteps

		public bool ShowSteps
		{
			get { return (bool)this.GetValue(ShowStepsProperty); }
			set { this.SetValue(ShowStepsProperty, value); }
		}

		public static readonly DependencyProperty ShowStepsProperty =
			DependencyProperty.Register(
				"ShowSteps",
				typeof(bool),
				typeof(ControlPanel));

		#endregion ShowSteps

	}
}
