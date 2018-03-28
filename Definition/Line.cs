using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Definition
{
	public sealed class Line : LineBase
	{
		internal static Line Row(int index, GridLine segment1, GridLine segment2, GridLine segment3)
		{
			return new Line(index, segment1, segment2, segment3)
			{
				lineType = LineType.Row
			};
		}

		internal static Line Column(int index, GridLine segment1, GridLine segment2, GridLine segment3)
		{
			return new Line(index, segment1, segment2, segment3)
			{
				lineType = LineType.Column
			};
		}

		private Line(int index, GridLine segment1, GridLine segment2, GridLine segment3)
		{
			this.index = index;

			this.segments = new GridLine[3] { segment1, segment2, segment3 };
			this.elements = new Element[9]
			{
				segment1.Element1, segment1.Element2, segment1.Element3,
				segment2.Element1, segment2.Element2, segment2.Element3,
				segment3.Element1, segment3.Element2, segment3.Element3
			};
			this.segment1 = segment1;
			this.segment2 = segment2;
			this.segment3 = segment3;
		}

		private GridLine[] segments;
		public GridLine[] Segments
		{
			get { return this.segments; }
		}

		private GridLine segment1;
		public GridLine Segment1
		{
			get { return this.segment1; }
		}

		private GridLine segment2;
		public GridLine Segment2
		{
			get { return this.segment2; }
		}

		private GridLine segment3;
		public GridLine Segment3
		{
			get { return this.segment3; }
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}
	}
}
