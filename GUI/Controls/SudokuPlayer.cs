using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuSolver.GUI
{
	public class SudokuPlayer : Control
	{
		public Model.Sudoku Sudoku
		{
			get { return (Model.Sudoku)this.GetValue(SudokuProperty); }
			set { this.SetValue(SudokuProperty, value); }
		}

		public static readonly DependencyProperty SudokuProperty =
			DependencyProperty.Register(
				"Sudoku",
				typeof(Model.Sudoku),
				typeof(SudokuPlayer));

		public Brush ElementBorderBrush
		{
			get { return (Brush)this.GetValue(ElementBorderBrushProperty); }
			set { this.SetValue(ElementBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty ElementBorderBrushProperty =
			DependencyProperty.Register(
				"ElementBorderBrush",
				typeof(Brush),
				typeof(SudokuPlayer));

		public Brush GridBorderBrush
		{
			get { return (Brush)this.GetValue(GridBorderBrushProperty); }
			set { this.SetValue(GridBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty GridBorderBrushProperty =
			DependencyProperty.Register(
				"GridBorderBrush",
				typeof(Brush),
				typeof(SudokuPlayer));

		public Brush SudokuBorderBrush
		{
			get { return (Brush)this.GetValue(SudokuBorderBrushProperty); }
			set { this.SetValue(SudokuBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty SudokuBorderBrushProperty =
			DependencyProperty.Register(
				"SudokuBorderBrush",
				typeof(Brush),
				typeof(SudokuPlayer));

		public double ElementSize
		{
			get { return (double)this.GetValue(ElementSizeProperty); }
			set { this.SetValue(ElementSizeProperty, value); }
		}

		public static readonly DependencyProperty ElementSizeProperty =
			DependencyProperty.Register(
				"ElementSize",
				typeof(double),
				typeof(SudokuPlayer));

		public double ElementBorderWidth
		{
			get { return (double)this.GetValue(ElementBorderWidthProperty); }
			set { this.SetValue(ElementBorderWidthProperty, value); }
		}

		public static readonly DependencyProperty ElementBorderWidthProperty =
			DependencyProperty.Register(
				"ElementBorderWidth",
				typeof(double),
				typeof(SudokuPlayer));

		public double GridBorderWidth
		{
			get { return (double)this.GetValue(GridBorderWidthProperty); }
			set { this.SetValue(GridBorderWidthProperty, value); }
		}

		public static readonly DependencyProperty GridBorderWidthProperty =
			DependencyProperty.Register(
				"GridBorderWidth",
				typeof(double),
				typeof(SudokuPlayer));

		public double SudokuBorderWidth
		{
			get { return (double)this.GetValue(SudokuBorderWidthProperty); }
			set { this.SetValue(SudokuBorderWidthProperty, value); }
		}

		public static readonly DependencyProperty SudokuBorderWidthProperty =
			DependencyProperty.Register(
				"SudokuBorderWidth",
				typeof(double),
				typeof(SudokuPlayer));

		#region BorderMargin

		public double GapWidth
		{
			get { return (double)this.GetValue(GapWidthProperty); }
			set { this.SetValue(GapWidthProperty, value); }
		}

		public static readonly DependencyProperty GapWidthProperty =
			DependencyProperty.Register(
				"GapWidth",
				typeof(double),
				typeof(SudokuPlayer),
				new PropertyMetadata(OnGapWidthPropertyChanged));

		private static void OnGapWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetValue(BorderMarginPropertyKey, new Thickness((double)e.NewValue));
		}

		public Thickness BorderMargin
		{
			get { return (Thickness)GetValue(BorderMarginProperty); }
		}

		public static readonly DependencyPropertyKey BorderMarginPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"BorderMargin",
				typeof(Thickness),
				typeof(SudokuPlayer),
				new PropertyMetadata());

		public static readonly DependencyProperty BorderMarginProperty =
			BorderMarginPropertyKey.DependencyProperty;

		private void SetBorderMargin(Thickness value)
		{
			SetValue(BorderMarginPropertyKey, value);
		}

		#endregion BorderMargin

	}
}
