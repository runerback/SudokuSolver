using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace SudokuSolver.GUI
{
	public class ElementPresenter : Control
	{
		public static readonly DependencyProperty GapWidthProperty =
			SudokuPlayer.GapWidthProperty.AddOwner(typeof(ElementPresenter));

		public static readonly DependencyProperty BorderMarginProperty =
			SudokuPlayer.BorderMarginProperty.AddOwner(typeof(ElementPresenter));

		public static readonly DependencyProperty ElementSizeProperty =
			SudokuPlayer.ElementSizeProperty.AddOwner(typeof(ElementPresenter));

		public static readonly DependencyProperty ElementBorderBrushProperty =
			SudokuPlayer.ElementBorderBrushProperty.AddOwner(typeof(ElementPresenter));

		public static readonly DependencyProperty ElementBorderWidthProperty =
			SudokuPlayer.ElementBorderWidthProperty.AddOwner(typeof(ElementPresenter));

		public Model.Element Element
		{
			get { return (Model.Element)this.GetValue(ElementProperty); }
			set { this.SetValue(ElementProperty, value); }
		}

		public static readonly DependencyProperty ElementProperty =
			DependencyProperty.Register(
				"Element",
				typeof(Model.Element),
				typeof(ElementPresenter));

	}
}
