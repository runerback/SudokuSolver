using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace SudokuSolver.GUI
{
	public class SudokuPlayer : Window
	{
		internal SudokuPlayer()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
		}

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

		public static SudokuPlayerController Show(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			if (originSudoku == null)
				throw new ArgumentNullException("originSudoku");
			if (playingSudoku == null)
				throw new ArgumentNullException("playingSudoku");

			Thread thread = new Thread(() =>
			{
				try
				{
					new App(createPlayer(originSudoku, playingSudoku)).Run();
				}
				catch (Exception exp)
				{
					Console.WriteLine(exp);
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Name = "sudoku player thread";
			thread.Start();

			throw new NotImplementedException();
		}

		private Business.SudokuAutoSync originSudokuAutoSync;

		private Business.SudokuStepSync playingSudokuStepSync;
		internal Business.SudokuStepSync PlayingSudokuStepSync
		{
			get { return this.playingSudokuStepSync; }
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			this.originSudokuAutoSync.Dispose();
			this.playingSudokuStepSync.Dispose();
		}

		private static SudokuPlayer createPlayer(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			if (originSudoku == null)
				throw new ArgumentNullException("originSudoku");
			if (playingSudoku == null)
				throw new ArgumentNullException("playingSudoku");

			SudokuPlayer player = new SudokuPlayer();

			var originSudokuModel = new Model.Sudoku();
			originSudokuModel.Sync(originSudoku);
			player.originSudokuAutoSync = new Business.SudokuAutoSync(originSudoku, originSudokuModel);

			var playingSudokuModel = new Model.Sudoku();
			playingSudokuModel.Sync(playingSudoku);
			player.playingSudokuStepSync = new Business.SudokuStepSync(playingSudoku, playingSudokuModel);

			player.OriginSudoku = originSudokuModel;
			player.PlayingSudoku = playingSudokuModel;

			return player;
		}
	}
}
