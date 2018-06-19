using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SudokuSolver.GUI
{
	public sealed class SudokuPlayerController
	{
		public SudokuPlayerController(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			this.window = SudokuPlayer.Create(originSudoku, playingSudoku);
		}

		private readonly SudokuPlayer window;

		public void Show()
		{
			var window = this.window;
			Action invoke = window.Show;
			window.Dispatcher.BeginInvoke(invoke);
		}

		public void ShowAndWaitForClose()
		{
			var window = this.window;
			Func<bool?> invoke = window.ShowDialog;
			window.Dispatcher.Invoke(invoke);
		}

		public void Close()
		{
			var window = this.window;
			Action invoke = window.Close;
			window.Dispatcher.Invoke(invoke);
		}

		public void Shutdown()
		{
			Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
		}
	}
}
