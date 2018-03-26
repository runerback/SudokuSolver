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
		public SudokuPlayer()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
		}

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

		public static SudokuPlayerController Show(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			Application app = null;
			ManualResetEvent blocker = new ManualResetEvent(false);

			Thread thread = new Thread(() =>
			{
				try
				{
					app = createShell(originSudoku, playingSudoku);
					blocker.Set();
					app.Run();
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
			blocker.Dispose();
			return new SudokuPlayerController(app);
		}

		private static Application createShell(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			if (originSudoku == null)
				throw new ArgumentNullException("sudoku");

			System.Windows.Application app = new System.Windows.Application();

			var controlTemplatesLocater = new Uri("/SudokuSolver.GUI;component/Resource/ControlTemplates.xaml", UriKind.Relative);
			app.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(controlTemplatesLocater));

			app.Startup += delegate
			{
				var gui = new SudokuPlayer
				{
					OriginSudoku = new Model.Sudoku(originSudoku),
					PlayingSudoku = new Model.Sudoku(playingSudoku)
				};

				gui.KeyUp += (o, e) =>
				{
					if (e.Key == System.Windows.Input.Key.Enter)
					{
						((Window)o).Close();
					}
				};

				gui.Show();
			};

			return app;
		}
	}
}
