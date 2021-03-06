﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Definition
{
	public sealed class Grid : IElementCluster
	{
		internal Grid(Sudoku sudoku, int index)
		{
			this.sudoku = sudoku;
			this.index = index;

			var elements = Enumerable.Range(0, 9)
				.Select(item => new Element(index, item))
				.ToArray();

			var rows = new GridLine[3]
			{
				GridLine.Row(this, 0, elements[0], elements[1], elements[2]),
				GridLine.Row(this, 1, elements[3], elements[4], elements[5]),
				GridLine.Row(this, 2, elements[6], elements[7], elements[8])
			};
			this.row1 = rows[0];
			this.row2 = rows[1];
			this.row3 = rows[2];
			this.rows = rows;

			var columns = new GridLine[3]
			{
				GridLine.Column(this, 0, elements[0], elements[3], elements[6]),
				GridLine.Column(this, 1, elements[1], elements[4], elements[7]),
				GridLine.Column(this, 2, elements[2], elements[5], elements[8])
			};
			this.column1 = columns[0];
			this.column2 = columns[1];
			this.column3 = columns[2];
			this.columns = columns;

			this.elements = elements;
		}

		private Element[] elements;
		public Element[] Elements
		{
			get { return elements; }
		}

		#region Row

		private GridLine[] rows;
		public GridLine[] Rows
		{
			get { return this.rows; }
		}

		private GridLine row1;
		public GridLine Row1
		{
			get { return this.row1; }
		}

		private GridLine row2;
		public GridLine Row2
		{
			get { return this.row2; }
		}

		private GridLine row3;
		public GridLine Row3
		{
			get { return this.row3; }
		}

		#endregion Row

		#region Column

		private GridLine[] columns;
		public GridLine[] Columns
		{
			get { return this.columns; }
		}

		private GridLine column1;
		public GridLine Column1
		{
			get { return this.column1; }
		}

		private GridLine column2;
		public GridLine Column2
		{
			get { return this.column2; }
		}

		private GridLine column3;
		public GridLine Column3
		{
			get { return this.column3; }
		}

		#endregion Column

		private Sudoku sudoku;
		public Sudoku Sudoku
		{
			get { return this.sudoku; }
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		public override int GetHashCode()
		{
			return index.GetHashCode();
		}

		public override string ToString()
		{
			return string.Join("\r\n", rows.AsEnumerable());
		}

		IEnumerable<Element> IElementCluster.Elements
		{
			get { return this.elements; }
		}
	}
}
