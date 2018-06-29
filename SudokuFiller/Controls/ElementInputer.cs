using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuFiller
{
	sealed class ElementInputer : Control
	{
		public ElementInputer()
		{

		}

		#region Value

		public int? Value
		{
			get { return (int?)this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(
				"Value",
				typeof(int?),
				typeof(ElementInputer));

		#endregion Value

		#region CaretBrush

		public Brush CaretBrush
		{
			get { return (Brush)this.GetValue(CaretBrushProperty); }
			set { this.SetValue(CaretBrushProperty, value); }
		}

		public static readonly DependencyProperty CaretBrushProperty =
			DependencyProperty.Register(
				"CaretBrush",
				typeof(Brush),
				typeof(ElementInputer));

		#endregion CaretBrush

	}
}
