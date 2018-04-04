using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SudokuSolver.GUI
{
	internal sealed class GridPresenter : Control
	{
		#region BorderMargin

		public Thickness BorderMargin
		{
			get { return (Thickness)this.GetValue(BorderMarginProperty); }
			set { this.SetValue(BorderMarginProperty, value); }
		}

		public static readonly DependencyProperty BorderMarginProperty =
			DependencyProperty.Register(
				"BorderMargin",
				typeof(Thickness),
				typeof(GridPresenter));

		#endregion BorderMargin

		#region ElementSize

		public double ElementSize
		{
			get { return (double)this.GetValue(ElementSizeProperty); }
			set { this.SetValue(ElementSizeProperty, value); }
		}

		public static readonly DependencyProperty ElementSizeProperty =
			DependencyProperty.Register(
				"ElementSize",
				typeof(double),
				typeof(GridPresenter));

		#endregion ElementSize

		#region ElementBorderBrush

		public Brush ElementBorderBrush
		{
			get { return (Brush)this.GetValue(ElementBorderBrushProperty); }
			set { this.SetValue(ElementBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty ElementBorderBrushProperty =
			DependencyProperty.Register(
				"ElementBorderBrush",
				typeof(Brush),
				typeof(GridPresenter));

		#endregion ElementBorderBrush

		#region ElementBorderWidth

		public double ElementBorderWidth
		{
			get { return (double)this.GetValue(ElementBorderWidthProperty); }
			set { this.SetValue(ElementBorderWidthProperty, value); }
		}

		public static readonly DependencyProperty ElementBorderWidthProperty =
			DependencyProperty.Register(
				"ElementBorderWidth",
				typeof(double),
				typeof(GridPresenter));

		#endregion ElementBorderWidth

		#region GridBorderBrush

		public Brush GridBorderBrush
		{
			get { return (Brush)this.GetValue(GridBorderBrushProperty); }
			set { this.SetValue(GridBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty GridBorderBrushProperty =
			DependencyProperty.Register(
				"GridBorderBrush",
				typeof(Brush),
				typeof(GridPresenter));

		#endregion GridBorderBrush

		#region GridBorderWidth

		public double GridBorderWidth
		{
			get { return (double)this.GetValue(GridBorderWidthProperty); }
			set { this.SetValue(GridBorderWidthProperty, value); }
		}

		public static readonly DependencyProperty GridBorderWidthProperty =
			DependencyProperty.Register(
				"GridBorderWidth",
				typeof(double),
				typeof(GridPresenter));

		#endregion GridBorderWidth

		#region GridBorderThickess

		public Thickness GridBorderThickness
		{
			get { return (Thickness)this.GetValue(GridBorderThicknessProperty); }
			internal set { this.SetValue(GridBorderThicknessProperty, value); }
		}

		public static readonly DependencyProperty GridBorderThicknessProperty =
			DependencyProperty.Register(
				"GridBorderThickness",
				typeof(Thickness),
				typeof(GridPresenter));

		private void UpdateGridBorderThickness(int gridIndex)
		{
			Converter.ThicknessConverterParameter parameter;
			switch (gridIndex)
			{
				case 0:
				case 1:
				case 3:
				case 4:
					parameter = new Converter.ThicknessConverterParameter(true, true, false, false);
					break;
				case 2:
				case 5:
					parameter = new Converter.ThicknessConverterParameter(true, true, true, false);
					break;
				case 6:
				case 7:
					parameter = new Converter.ThicknessConverterParameter(true, true, false, true);
					break;
				case 8:
					parameter = new Converter.ThicknessConverterParameter(true);
					break;
				default:
					parameter = new Converter.ThicknessConverterParameter(false);
					break;
			}

			BindingOperations.SetBinding(
				this,
				GridBorderThicknessProperty,
				new Binding()
				{
					Path = new PropertyPath(GridBorderWidthProperty),
					Source = this,
					Mode = BindingMode.OneWay,
					Converter = new Converter.ThicknessConverter(),
					ConverterParameter = parameter
				});
		}

		#endregion GridBorderThickess

		#region Grid

		public Model.Grid Grid
		{
			get { return (Model.Grid)this.GetValue(GridProperty); }
			set { this.SetValue(GridProperty, value); }
		}

		public static readonly DependencyProperty GridProperty =
			DependencyProperty.Register(
				"Grid",
				typeof(Model.Grid),
				typeof(GridPresenter),
				new PropertyMetadata(onGridPropertyChanged));

		private static void onGridPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((GridPresenter)d).UpdateGridBorderThickness(e.NewValue == null ?
				-1 :
				((Model.Grid)e.NewValue).Index);
		}

		#endregion Grid

	}
}
