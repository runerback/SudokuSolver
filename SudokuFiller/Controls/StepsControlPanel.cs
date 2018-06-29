using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SudokuFiller
{
	sealed class StepsControlPanel : Control
	{
		public StepsControlPanel()
		{

		}

		#region ShowPreviousCommand

		public ICommand ShowPreviousCommand
		{
			get { return (ICommand)this.GetValue(ShowPreviousCommandProperty); }
			set { this.SetValue(ShowPreviousCommandProperty, value); }
		}

		public static readonly DependencyProperty ShowPreviousCommandProperty =
			DependencyProperty.Register(
				"ShowPreviousCommand",
				typeof(ICommand),
				typeof(StepsControlPanel));

		#endregion ShowPreviousCommand

		#region ShowNextCommand

		public ICommand ShowNextCommand
		{
			get { return (ICommand)this.GetValue(ShowNextCommandProperty); }
			set { this.SetValue(ShowNextCommandProperty, value); }
		}

		public static readonly DependencyProperty ShowNextCommandProperty =
			DependencyProperty.Register(
				"ShowNextCommand",
				typeof(ICommand),
				typeof(StepsControlPanel));

		#endregion ShowNextCommand
		
		#region HideCommand

		public ICommand HideCommand
		{
			get { return (ICommand)this.GetValue(HideCommandProperty); }
			set { this.SetValue(HideCommandProperty, value); }
		}

		public static readonly DependencyProperty HideCommandProperty =
			DependencyProperty.Register(
				"HideCommand",
				typeof(ICommand),
				typeof(StepsControlPanel));

		#endregion HideCommand

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

		#region Steps

		public SudokuSolveResult Steps
		{
			get { return (SudokuSolveResult)this.GetValue(StepsProperty); }
			set { this.SetValue(StepsProperty, value); }
		}

		public static readonly DependencyProperty StepsProperty =
			DependencyProperty.Register(
				"Steps",
				typeof(SudokuSolveResult),
				typeof(StepsControlPanel));

		#endregion Steps

	}
}
