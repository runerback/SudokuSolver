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
		internal SudokuPlayerController(Application app)
		{
			if (app == null)
				throw new ArgumentNullException("app");
			this.dispatcher = app.Dispatcher;
		}

		private Dispatcher dispatcher;
		private bool closed = false;

		public void Close()
		{
			if (!closed)
			{
				if(this.dispatcher!=null)
					dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
				closed = true;
			}
		}

		#region StepView

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
