using System.Windows;

namespace SudokuSolver.GUI.Converter
{
	public class ThicknessConverterParameter
	{
		public ThicknessConverterParameter(bool hasLeft, bool hasTop, bool hasRight, bool hasBottom)
		{
			this.hasLeft = hasLeft;
			this.hasRight = hasRight;
			this.hasTop = hasTop;
			this.hasBottom = hasBottom;
		}

		public ThicknessConverterParameter(bool hasAll)
		{
			this.hasLeft = hasAll;
			this.hasRight = hasAll;
			this.hasTop = hasAll;
			this.hasBottom = hasAll;
		}

		private bool hasLeft;
		private bool hasRight;
		private bool hasTop;
		private bool hasBottom;

		public Thickness GetThickness(double baseValue)
		{
			return new Thickness(
				hasLeft ? baseValue : 0,
				hasTop ? baseValue : 0,
				hasRight ? baseValue : 0,
				hasBottom ? baseValue : 0);
		}
	}
}
