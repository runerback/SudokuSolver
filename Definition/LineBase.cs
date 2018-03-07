using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Definition
{
	public abstract class LineBase
	{
		protected Element[] elements;
		public Element[] Elements
		{
			get { return elements; }
		}


		protected LineType lineType;
		public LineType LineType
		{
			get { return this.lineType; }
		}

		public override string ToString()
		{
			string spliter = null;
			switch (lineType)
			{
				case Definition.LineType.Row:
					spliter = " ";
					break;
				case Definition.LineType.Column:
					spliter = "\r\n";
					break;
				default: return base.ToString();
			}
			return string.Join(spliter, elements.AsEnumerable());
		}
	}
}
