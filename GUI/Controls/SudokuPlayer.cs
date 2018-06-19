using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SudokuSolver.GUI
{
	internal sealed class SudokuPlayer : Window
	{
		private SudokuPlayer(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			if (playingSudoku == null)
				throw new ArgumentNullException("playingSudoku");

			if (originSudoku != null)
			{
				var originSudokuModel = new Model.Sudoku();
				originSudokuModel.Sync(originSudoku);
				this.originSudokuAutoSync = new Business.SudokuAutoSync(originSudoku, originSudokuModel);
				this.OriginSudoku = originSudokuModel;
			}
			else
			{
				this.SetOriginSudokuVisibility(System.Windows.Visibility.Collapsed);
			}

			this.steps = new Business.SudokuSolveStepCollection();

			var playingSudokuModel = new Model.Sudoku();
			playingSudokuModel.Sync(playingSudoku);
			var playingSudokuStepSync = new Business.SudokuStepSync(playingSudoku, playingSudokuModel);
			playingSudokuStepSync.NewStep += onNewStep;
			this.playingSudokuStepSync = playingSudokuStepSync;

			this.PlayingSudoku = playingSudokuModel;
		}

		#region OriginSudokuVisibility

		public Visibility OriginSudokuVisibility
		{
			get { return (Visibility)GetValue(OriginSudokuVisibilityProperty); }
		}

		static readonly DependencyPropertyKey OriginSudokuVisibilityPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"OriginSudokuVisibility",
				typeof(Visibility),
				typeof(SudokuPlayer),
				new PropertyMetadata(Visibility.Visible));

		public static readonly DependencyProperty OriginSudokuVisibilityProperty =
			OriginSudokuVisibilityPropertyKey.DependencyProperty;

		private void SetOriginSudokuVisibility(Visibility value)
		{
			SetValue(OriginSudokuVisibilityPropertyKey, value);
		}
		
		#endregion OriginSudokuVisibility

		#region OriginSudoku

		public Model.Sudoku OriginSudoku
		{
			get { return (Model.Sudoku)this.GetValue(OriginSudokuProperty); }
			set { this.SetValue(OriginSudokuProperty, value); }
		}

		public static readonly DependencyProperty OriginSudokuProperty =
			DependencyProperty.Register(
				"OriginSudoku",
				typeof(Model.Sudoku),
				typeof(SudokuPlayer));

		#endregion OriginSudoku

		#region PlayingSudoku

		public Model.Sudoku PlayingSudoku
		{
			get { return (Model.Sudoku)this.GetValue(PlayingSudokuProperty); }
			set { this.SetValue(PlayingSudokuProperty, value); }
		}

		public static readonly DependencyProperty PlayingSudokuProperty =
			DependencyProperty.Register(
				"PlayingSudoku",
				typeof(Model.Sudoku),
				typeof(SudokuPlayer));

		#endregion PlayingSudoku

		static SudokuPlayer()
		{
			App app = null;
			using (ManualResetEvent blocker = new ManualResetEvent(false))
			{
				var thread = new Thread(() =>
					{
						try
						{
							app = new App { ShutdownMode = ShutdownMode.OnExplicitShutdown };
							blocker.Set();
							SudokuPlayer.app.Run();
						}
						catch (Exception exp)
						{
							Console.WriteLine(exp);
						}
					});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Name = "sudoku player thread";
				thread.Start();

				blocker.WaitOne();
			}
			SudokuPlayer.app = app;
		}

		private readonly static App app;

		public static SudokuPlayer Create(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			if (playingSudoku == null)
				throw new ArgumentNullException("playingSudoku");

			return (SudokuPlayer)app.Dispatcher.Invoke((Func<SudokuPlayer>)delegate
			{
				return new SudokuPlayer(originSudoku, playingSudoku);
			});
		}

		private Business.SudokuAutoSync originSudokuAutoSync;
		private Business.SudokuStepSync playingSudokuStepSync;

		private Business.SudokuSolveStepCollection steps;

		private void onNewStep(object sender, Business.SudokuNewStepEventArgs e)
		{
			this.steps.Add(e.Step);
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			base.OnClosing(e);

			if(this.originSudokuAutoSync!=null)
				this.originSudokuAutoSync.Dispose();
			this.playingSudokuStepSync.Dispose();
		}

		#region Controller

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);

			switch (e.Key)
			{
				case Key.Left:
					ShowPreviousStep();
					break;
				case Key.Right:
					ShowNextStep();
					break;
				case Key.Escape:
					this.Close();
					break;
				default: break;
			}
		}

		private bool canMovePrevious = true;
		private void ShowPreviousStep()
		{
			if (!this.steps.Previous())
			{
				if (canMovePrevious)
				{
					canMovePrevious = false;
					SetStatus("no more steps in front");
				}
			}
			else
			{
				SetStatus(null);
				if (!canMovePrevious)
				{
					canMovePrevious = true;
				}
			}
			canMoveNext = true;
		}

		private bool canMoveNext = true;
		private void ShowNextStep()
		{
			if (!this.steps.Next())
			{
				if (canMoveNext)
				{
					canMoveNext = false;
					SetStatus("no more steps in behind");
				}
			}
			else
			{
				SetStatus(null);
				if (!canMoveNext)
				{
					canMoveNext = true;
				}
			}
			canMovePrevious = true;
		}

		#endregion Controller

		#region Status

		public string Status
		{
			get { return (string)GetValue(StatusProperty); }
		}

		public static readonly DependencyPropertyKey StatusPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"Status",
				typeof(string),
				typeof(SudokuPlayer),
				new PropertyMetadata());

		public static readonly DependencyProperty StatusProperty =
			StatusPropertyKey.DependencyProperty;

		private void SetStatus(string value)
		{
			SetValue(StatusPropertyKey, value);
		}

		#endregion Status

	}
}
