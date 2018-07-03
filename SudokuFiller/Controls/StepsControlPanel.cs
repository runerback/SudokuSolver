using Runerback.Utils.Wpf;
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
			this.showPreviousCommand = new DelegateCommand(showPreviousStep, canShowPreviousStep);
			SetShowPreviousCommand(this.showPreviousCommand);

			this.showFirstCommand = new DelegateCommand(showFirstStep, canShowFirstStep);
			SetShowFirstCommand(this.showFirstCommand);

			this.showNextCommand = new DelegateCommand(showNextStep, canShowNextStep);
			SetShowNextCommand(this.showNextCommand);

			this.showLastCommand = new DelegateCommand(showLastStep, canShowLastStep);
			SetShowLastCommand(this.showLastCommand);

			this.hideCommand = new DelegateCommand(hide, canHide);
			SetHideCommand(this.hideCommand);
		}
		
		#region showPreviousCommand

		private readonly DelegateCommand showPreviousCommand;

		private bool canShowPreviousStep(object obj)
		{
			if (this.Steps == null)
				return false;
			return this.Steps.HasPreviousStep();
		}

		private void showPreviousStep(object obj)
		{
			if (this.Steps == null)
				throw new InvalidOperationException("no steps");
			this.Steps.PreviousStep();
			updateStepCommandsState();
		}

		#endregion showPreviousCommand

		#region ShowPreviousCommand

		public ICommand ShowPreviousCommand
		{
			get { return (ICommand)GetValue(ShowPreviousCommandProperty); }
		}

		static readonly DependencyPropertyKey ShowPreviousCommandPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"ShowPreviousCommand",
				typeof(ICommand),
				typeof(StepsControlPanel),
				new PropertyMetadata());

		public static readonly DependencyProperty ShowPreviousCommandProperty =
			ShowPreviousCommandPropertyKey.DependencyProperty;

		private void SetShowPreviousCommand(ICommand value)
		{
			SetValue(ShowPreviousCommandPropertyKey, value);
		}

		#endregion ShowPreviousCommand

		#region showFirstCommand

		private readonly DelegateCommand showFirstCommand;

		private bool canShowFirstStep(object obj)
		{
			if (this.Steps == null)
				return false;
			return this.Steps.HasPreviousStep();
		}

		private void showFirstStep(object obj)
		{
			if (this.Steps == null)
				throw new InvalidOperationException("no steps");
			this.Steps.FirstStep();
			updateStepCommandsState();
		}

		#endregion showFirstCommand

		#region ShowFirstStepCommand

		public ICommand ShowFirstCommand
		{
			get { return (ICommand)GetValue(ShowFirstCommandProperty); }
		}

		static readonly DependencyPropertyKey ShowFirstCommandPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"ShowFirstCommand",
				typeof(ICommand),
				typeof(StepsControlPanel),
				new PropertyMetadata());

		public static readonly DependencyProperty ShowFirstCommandProperty =
			ShowFirstCommandPropertyKey.DependencyProperty;

		private void SetShowFirstCommand(ICommand value)
		{
			SetValue(ShowFirstCommandPropertyKey, value);
		}

		#endregion ShowFirstStepCommand

		#region showNextCommand

		private readonly DelegateCommand showNextCommand;

		private bool canShowNextStep(object obj)
		{
			if (this.Steps == null)
				return false;
			return this.Steps.HasNextStep();
		}

		private void showNextStep(object obj)
		{
			if (this.Steps == null)
				throw new InvalidOperationException("no steps");
			this.Steps.NextStep();
			updateStepCommandsState();
		}

		#endregion showNextCommand
		
		#region ShowNextCommand

		public ICommand ShowNextCommand
		{
			get { return (ICommand)GetValue(ShowNextCommandProperty); }
		}

		static readonly DependencyPropertyKey ShowNextCommandPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"ShowNextCommand",
				typeof(ICommand),
				typeof(StepsControlPanel),
				new PropertyMetadata());

		public static readonly DependencyProperty ShowNextCommandProperty =
			ShowNextCommandPropertyKey.DependencyProperty;

		private void SetShowNextCommand(ICommand value)
		{
			SetValue(ShowNextCommandPropertyKey, value);
		}
		
		#endregion ShowNextCommand

		#region showLastCommand

		private readonly DelegateCommand showLastCommand;

		private bool canShowLastStep(object obj)
		{
			if (this.Steps == null)
				return false;
			return this.Steps.HasNextStep();
		}

		private void showLastStep(object obj)
		{
			if (this.Steps == null)
				throw new InvalidOperationException("no steps");
			this.Steps.LastStep();
			updateStepCommandsState();
		}

		#endregion showLastCommand

		#region ShowLastCommand

		public ICommand ShowLastCommand
		{
			get { return (ICommand)GetValue(ShowLastCommandProperty); }
		}

		static readonly DependencyPropertyKey ShowLastCommandPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"ShowLastCommand",
				typeof(ICommand),
				typeof(StepsControlPanel),
				new PropertyMetadata());

		public static readonly DependencyProperty ShowLastCommandProperty =
			ShowLastCommandPropertyKey.DependencyProperty;

		private void SetShowLastCommand(ICommand value)
		{
			SetValue(ShowLastCommandPropertyKey, value);
		}
		
		#endregion ShowLastCommand

		private void updateStepCommandsState()
		{
			this.showPreviousCommand.NotifyCanExecuteChanged();
			this.showFirstCommand.NotifyCanExecuteChanged();

			this.showNextCommand.NotifyCanExecuteChanged();
			this.showLastCommand.NotifyCanExecuteChanged();
		}

		#region hideCommand

		private readonly DelegateCommand hideCommand;


		private bool canHide(object obj)
		{
			return this.ShowSteps;
		}

		private void hide(object obj)
		{
			this.ShowSteps = false;
		}

		#endregion hideCommand

		#region HideCommand

		public ICommand HideCommand
		{
			get { return (ICommand)GetValue(HideCommandProperty); }
		}

		static readonly DependencyPropertyKey HideCommandPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"HideCommand",
				typeof(ICommand),
				typeof(StepsControlPanel),
				new PropertyMetadata());

		public static readonly DependencyProperty HideCommandProperty =
			HideCommandPropertyKey.DependencyProperty;

		private void SetHideCommand(ICommand value)
		{
			SetValue(HideCommandPropertyKey, value);
		}

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
				typeof(StepsControlPanel),
				new PropertyMetadata(OnShowStepsPropertyChanged));

		private static void OnShowStepsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var self = (StepsControlPanel)d;

			self.hideCommand.NotifyCanExecuteChanged();
			if ((bool)e.NewValue)
				self.updateStepCommandsState();
		}

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
				typeof(StepsControlPanel),
				new PropertyMetadata(OnStepsPropertyChanged));

		private static void OnStepsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var value = e.NewValue as SudokuSolveResult;
			if (value != null && value.State != SudokuSolveState.Solved)
			{
				d.SetValue(StepsProperty, null);
				return;
			}

			var self = (StepsControlPanel)d;
			self.hideCommand.NotifyCanExecuteChanged();
			self.updateStepCommandsState();
		}

		#endregion Steps

	}
}
