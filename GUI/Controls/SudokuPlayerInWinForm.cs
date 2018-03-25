using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver.GUI
{
	public class SudokuPlayerInWinForm : Panel
	{
		public SudokuPlayerInWinForm()
		{
			this.BackColor = Color.WhiteSmoke;
			this.ForeColor = Color.Black;

			this.AutoSize = false;

			int width = (SUDOKU_BORDER_WIDTH + GAP_WIDTH) * 2 +
				(GRID_BORDER_WIDTH + GAP_WIDTH) * 4 +
				(ELEMENT_SIZE + GAP_WIDTH) * 9 +
				(ELEMENT_BORDER_WIDTH + GAP_WIDTH) * 6;

			var margin = this.Margin;
			this.Size = new Size(width + margin.Left + margin.Right, width + margin.Top + margin.Bottom);

			this.DoubleBuffered = true;
		}

		private Color ElementBorderColor = Color.LightGray;
		private Color GridBorderColor = Color.LightSkyBlue;
		private Color SudokuBorderColor = Color.Gray;

		private const int ELEMENT_SIZE = 21;
		private const int ELEMENT_BORDER_WIDTH = 1;
		private const int GRID_BORDER_WIDTH = 2;
		private const int SUDOKU_BORDER_WIDTH = 5;
		private const int GAP_WIDTH = 1;

		private const int GRIDS_WIDTH = GRID_BORDER_WIDTH * 4 + (GAP_WIDTH * 3 + ELEMENT_BORDER_WIDTH) * 6 + ELEMENT_SIZE * 9;
		private const int GRID_BORDER_TOP_LEFT = SUDOKU_BORDER_WIDTH + GAP_WIDTH;
		private const int DIST_GRID_BORDER = GRID_BORDER_WIDTH + (GAP_WIDTH * 3 + ELEMENT_BORDER_WIDTH) * 2 + ELEMENT_SIZE * 3;

		private bool isFrameDrawn = false;

		protected override void OnPaint(PaintEventArgs e)
		{
			var graph = e.Graphics;

			if (!isFrameDrawn)
			{
				graph.FillRectangle(
					new SolidBrush(this.BackColor),
					ClientRectangle);

				var margin = this.Margin;


				var SudokuBorderPen = new Pen(SudokuBorderColor, SUDOKU_BORDER_WIDTH);
				graph.DrawRectangle(
					SudokuBorderPen,
					new Rectangle(margin.Left, margin.Top, this.Width - margin.Left - margin.Right, this.Height - margin.Top - margin.Bottom));

				var GridBorderPen = new Pen(GridBorderColor, GRID_BORDER_WIDTH);
				var ElementBorderPen = new Pen(ElementBorderColor, ELEMENT_BORDER_WIDTH);

				for (int i = 0; i < 4; i++)
				{
					//row
					graph.DrawLine(
						GridBorderPen,
						new Point(
							margin.Left + GRID_BORDER_TOP_LEFT,
							margin.Top + GRID_BORDER_TOP_LEFT + DIST_GRID_BORDER * i),
						new Point(
							margin.Left + GRID_BORDER_TOP_LEFT + GRIDS_WIDTH,
							margin.Top + GRID_BORDER_TOP_LEFT + DIST_GRID_BORDER * i)
					);

					//column
					graph.DrawLine(
						GridBorderPen,
						new Point(
							margin.Top + GRID_BORDER_TOP_LEFT + DIST_GRID_BORDER * i,
							margin.Left + GRID_BORDER_TOP_LEFT),
						new Point(
							margin.Top + GRID_BORDER_TOP_LEFT + DIST_GRID_BORDER * i,
							margin.Left + GRID_BORDER_TOP_LEFT + GRIDS_WIDTH)
					);

					for (int j = 0; j < 3; j++)
					{
						for (int k = 0; k < 3; k++)
						{
							//grid row

						}
					}
				}

				isFrameDrawn = true;
			}
		}
	}
}
