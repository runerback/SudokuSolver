using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SudokuFiller
{
	sealed class ElementInputer : Control
	{
		public ElementInputer()
		{

		}

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
	}
}
