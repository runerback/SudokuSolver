using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SudokuSolver.GUI
{
	public class SudokuPlayerController
	{
		internal SudokuPlayerController(SudokuPlayer player)
		{
			if (player == null)
				throw new ArgumentNullException("player");
			this.player = player;

			player.PlayingSudokuStepSync.NewStep += onNewStep;
		}

		private SudokuPlayer player;

		public void Close()
		{
			var player = this.player;
			player.Dispatcher.BeginInvoke((Action)player.Close);
		}

		#region StepView

		private void onNewStep(object sender, Business.SudokuNewStepEventArgs e)
		{
			this.steps.Add(e.Step);
		}

		private Business.SudokuSolveStepCollection steps = new Business.SudokuSolveStepCollection();
		internal Business.SudokuSolveStepCollection Steps
		{
			get { return this.steps; }
		}

		#region Play

		public bool PlayNext()
		{
			Business.SudokuSolveStep nextStep;
			if (this.steps.TryGetNextStep(out nextStep))
			{
				nextStep.Execute();
				return true;
			}
			return false;
		}

		public bool PlayCurrent()
		{
			Business.SudokuSolveStep currentStep;
			if (this.steps.TryGetCurrentStep(out currentStep))
			{
				currentStep.Execute();
				return true;
			}
			return false;
		}

		public bool PlayPrevious()
		{
			Business.SudokuSolveStep previousStep;
			if (this.steps.TryGetPreviousStep(out previousStep))
			{
				previousStep.Execute();
				return true;
			}
			return false;
		}

		#endregion Play

		#region Restore

		public void RestoreNext()
		{
			Business.SudokuSolveStep nextStep;
			if (this.steps.TryGetNextStep(out nextStep))
				nextStep.Restore();
		}

		public void RestoreCurrent()
		{
			Business.SudokuSolveStep currentStep;
			if (this.steps.TryGetCurrentStep(out currentStep))
				currentStep.Restore();
		}

		public void RestorePrevious()
		{
			Business.SudokuSolveStep previousStep;
			if (this.steps.TryGetPreviousStep(out previousStep))
				previousStep.Restore();
		}

		#endregion Restore

		#endregion StepView

	}
}
