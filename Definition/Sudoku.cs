using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Definition
{
	public class Sudoku
	{
		public Sudoku()
		{
			var grids = Enumerable.Range(0, 9)
				.Select(item => new Grid(this, item))
				.ToArray();

			var grid1 = grids[0];
			var grid2 = grids[1];
			var grid3 = grids[2];
			var grid4 = grids[3];
			var grid5 = grids[4];
			var grid6 = grids[5];
			var grid7 = grids[6];
			var grid8 = grids[7];
			var grid9 = grids[8];

			this.grids = grids;

			this.grid1 = grid1;
			this.grid2 = grid2;
			this.grid3 = grid3;
			this.grid4 = grid4;
			this.grid5 = grid5;
			this.grid6 = grid6;
			this.grid7 = grid7;
			this.grid8 = grid8;
			this.grid9 = grid9;

			var rows = new Line[9]
			{
				Line.Row(grid1.Row1, grid2.Row1, grid3.Row1),
				Line.Row(grid1.Row2, grid2.Row2, grid3.Row2),
				Line.Row(grid1.Row3, grid2.Row3, grid3.Row3),
				Line.Row(grid4.Row1, grid5.Row1, grid6.Row1),
				Line.Row(grid4.Row2, grid5.Row2, grid6.Row2),
				Line.Row(grid4.Row3, grid5.Row3, grid6.Row3),
				Line.Row(grid7.Row1, grid8.Row1, grid9.Row1),
				Line.Row(grid7.Row2, grid8.Row2, grid9.Row2),
				Line.Row(grid7.Row3, grid8.Row3, grid9.Row3)
			};
			this.rows = rows;
			this.row1 = rows[0];
			this.row2 = rows[1];
			this.row3 = rows[2];
			this.row4 = rows[3];
			this.row5 = rows[4];
			this.row6 = rows[5];
			this.row7 = rows[6];
			this.row8 = rows[7];
			this.row9 = rows[8];

			var columns = new Line[9]
			{
				Line.Column(grid1.Column1, grid4.Column1, grid7.Column1),
				Line.Column(grid1.Column2, grid4.Column2, grid7.Column2),
				Line.Column(grid1.Column3, grid4.Column3, grid7.Column3),
				Line.Column(grid2.Column1, grid5.Column1, grid8.Column1),
				Line.Column(grid2.Column2, grid5.Column2, grid8.Column2),
				Line.Column(grid2.Column3, grid5.Column3, grid8.Column3),
				Line.Column(grid3.Column1, grid6.Column1, grid9.Column1),
				Line.Column(grid3.Column2, grid6.Column2, grid9.Column2),
				Line.Column(grid3.Column3, grid6.Column3, grid9.Column3)
			};
			this.columns = columns;
			this.column1 = columns[0];
			this.column2 = columns[1];
			this.column3 = columns[2];
			this.column4 = columns[3];
			this.column5 = columns[4];
			this.column6 = columns[5];
			this.column7 = columns[6];
			this.column8 = columns[7];
			this.column9 = columns[8];
		}

		private Grid[] grids;
		public Grid[] Grids
		{
			get { return this.grids; }
		}

		#region Gird

		private Grid grid1;
		public Grid Grid1
		{
			get { return this.grid1; }
		}

		private Grid grid2;
		public Grid Grid2
		{
			get { return this.grid2; }
		}

		private Grid grid3;
		public Grid Grid3
		{
			get { return this.grid3; }
		}

		private Grid grid4;
		public Grid Grid4
		{
			get { return this.grid4; }
		}

		private Grid grid5;
		public Grid Grid5
		{
			get { return this.grid5; }
		}

		private Grid grid6;
		public Grid Grid6
		{
			get { return this.grid6; }
		}

		private Grid grid7;
		public Grid Grid7
		{
			get { return this.grid7; }
		}

		private Grid grid8;
		public Grid Grid8
		{
			get { return this.grid8; }
		}

		private Grid grid9;
		public Grid Grid9
		{
			get { return this.grid9; }
		}

		#endregion Gird

		#region Row

		private Line[] rows;
		public Line[] Rows
		{
			get { return this.rows; }
		}

		private Line row1;
		public Line Row1
		{
			get { return this.row1; }
		}

		private Line row2;
		public Line Row2
		{
			get { return this.row2; }
		}

		private Line row3;
		public Line Row3
		{
			get { return this.row3; }
		}

		private Line row4;
		public Line Row4
		{
			get { return this.row4; }
		}

		private Line row5;
		public Line Row5
		{
			get { return this.row5; }
		}

		private Line row6;
		public Line Row6
		{
			get { return this.row6; }
		}

		private Line row7;
		public Line Row7
		{
			get { return this.row7; }
		}

		private Line row8;
		public Line Row8
		{
			get { return this.row8; }
		}

		private Line row9;
		public Line Row9
		{
			get { return this.row9; }
		}

		#endregion Row

		#region Column

		private Line[] columns;
		public Line[] Columns
		{
			get { return this.columns; }
		}

		private Line column1;
		public Line Column1
		{
			get { return this.column1; }
		}

		private Line column2;
		public Line Column2
		{
			get { return this.column2; }
		}

		private Line column3;
		public Line Column3
		{
			get { return this.column3; }
		}

		private Line column4;
		public Line Column4
		{
			get { return this.column4; }
		}

		private Line column5;
		public Line Column5
		{
			get { return this.column5; }
		}

		private Line column6;
		public Line Column6
		{
			get { return this.column6; }
		}

		private Line column7;
		public Line Column7
		{
			get { return this.column7; }
		}

		private Line column8;
		public Line Column8
		{
			get { return this.column8; }
		}

		private Line column9;
		public Line Column9
		{
			get { return this.column9; }
		}

		#endregion Column

		public Sudoku Copy()
		{
			Sudoku copy = new Sudoku();

			for (int i = 0; i < 9; i++)
			{
				var sourceGrid = this.grids[i];
				var targetGrid = copy.grids[i];

				for (int j = 0; j < 9; j++)
				{
					var sourceElement = sourceGrid.Elements[j];
					var targetElement = targetGrid.Elements[j];

					if (sourceElement.HasValue)
					{
						targetElement.SetValue(sourceElement.Value.Value);
					}
					else
					{
						targetElement.ClearValue();
					}
				}
			}

			return copy;
		}

		public override string ToString()
		{
			return string.Join("\r\n", rows.AsEnumerable());
		}
	}
}
