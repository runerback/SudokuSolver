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
		public SudokuPlayerController(Definition.Sudoku originSudoku, Definition.Sudoku playingSudoku)
		{
			this.shell = SudokuPlayer.Create(originSudoku, playingSudoku);
		}

		private Window shell;

		public void Show()
		{
			var shell = this.shell;
			Action invoke = shell.Show;
			shell.Dispatcher.BeginInvoke(invoke);
		}

		public void ShowAndWaitForClose()
		{
			var shell = this.shell;
			Func<bool?> invoke = shell.ShowDialog;
			shell.Dispatcher.Invoke(invoke);
		}
	}
}
