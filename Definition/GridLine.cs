using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Definition
{
	public sealed class GridLine : LineBase
	{
		internal static GridLine Row(Element element1, Element element2, Element element3)
		{
			return new GridLine(element1, element2, element3)
			{
				lineType = LineType.Row
			};
		}

		internal static GridLine Column(Element element1, Element element2, Element element3)
		{
			return new GridLine(element1, element2, element3)
			{
				lineType = LineType.Column
			};
		}

		private GridLine(Element element1, Element element2, Element element3)
		{
			this.elements = new Element[] { element1, element2, element3 };
			this.element1 = element1;
			this.element2 = element2;
			this.element3 = element3;
		}

		private Element element1;
		public Element Element1
		{
			get { return this.element1; }
		}

		private Element element2;
		public Element Element2
		{
			get { return this.element2; }
		}

		private Element element3;
		public Element Element3
		{
			get { return this.element3; }
		}
	}
}
