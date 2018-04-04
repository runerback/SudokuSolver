using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuSolver.GUI
{
	internal sealed class ControllerTip : Control
	{
		public ImageSource Symbol
		{
			get { return (ImageSource)this.GetValue(SymbolProperty); }
			set { this.SetValue(SymbolProperty, value); }
		}

		public static readonly DependencyProperty SymbolProperty =
			DependencyProperty.Register(
				"Symbol",
				typeof(ImageSource),
				typeof(ControllerTip));

		public string Tip
		{
			get { return (string)this.GetValue(TipProperty); }
			set { this.SetValue(TipProperty, value); }
		}

		public static readonly DependencyProperty TipProperty =
			DependencyProperty.Register(
				"Tip",
				typeof(string),
				typeof(ControllerTip));

	}
}
