using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver.GUI
{
	class Program
	{
		static void Main()
		{
			Form shell = new Form
			{
				StartPosition = FormStartPosition.CenterScreen,
				FormBorderStyle = FormBorderStyle.FixedToolWindow,
				Text = "Sudoku"
			};
			var player = new SudokuPlayerInWinForm();
			shell.Controls.Add(player);
			player.Anchor = AnchorStyles.None;
			player.Left = (shell.DisplayRectangle.Width - player.Width) / 2;
			player.Top = (shell.DisplayRectangle.Height - player.Height) / 2;

			shell.PreviewKeyDown += (o, e) =>
			{
				if (e.KeyCode == Keys.Enter)
				{
					((Form)o).Close();
				}
			};
			Application.Run(shell);
		}
	}
}
