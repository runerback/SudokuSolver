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

		public void PlayNext()
		{
			throw new NotImplementedException();
		}

		public void PlayCurrent()
		{
			throw new NotImplementedException();
		}

		public void PlayPrevious()
		{
			throw new NotImplementedException();
		}

		#endregion Play

		#region Restore

		public void RestoreNext()
		{
			throw new NotImplementedException();
		}

		public void RestoreCurrent()
		{
			throw new NotImplementedException();
		}

		public void RestorePrevious()
		{
			throw new NotImplementedException();
		}

		#endregion Restore

		#endregion StepView

	}
}
