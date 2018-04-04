using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SudokuSolver.GUI
{
	internal sealed class ElementPresenter : Control
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
				typeof(ElementPresenter));

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
				typeof(ElementPresenter));

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
				typeof(ElementPresenter));

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
				typeof(ElementPresenter));

		#endregion ElementBorderWidth

		#region ElementBorderThickness

		public Thickness ElementBorderThickness
		{
			get { return (Thickness)GetValue(ElementBorderThicknessProperty); }
			internal set { SetValue(ElementBorderThicknessProperty, value); }
		}

		public static readonly DependencyProperty ElementBorderThicknessProperty =
			DependencyProperty.Register(
				"ElementBorderThickness",
				typeof(Thickness),
				typeof(ElementPresenter));

		private void UpdateElementBorderThickness(int elementIndex)
		{
			Converter.ThicknessConverterParameter parameter;
			switch (elementIndex)
			{
				case 0:
				case 1:
				case 3:
				case 4:
					parameter = new Converter.ThicknessConverterParameter(false, false, true, true);
					break;
				case 2:
				case 5:
					parameter = new Converter.ThicknessConverterParameter(false, false, false, true);
					break;
				case 6:
				case 7:
					parameter = new Converter.ThicknessConverterParameter(false, false, true, false);
					break;
				default:
					parameter = new Converter.ThicknessConverterParameter(false);
					break;
			}

			BindingOperations.SetBinding(
				this,
				ElementBorderThicknessProperty,
				new Binding()
				{
					Path = new PropertyPath(ElementBorderWidthProperty),
					Source = this,
					Mode = BindingMode.OneWay,
					Converter = new Converter.ThicknessConverter(),
					ConverterParameter = parameter
				});
		}

		#endregion ElementBorderThickness

		#region Element

		public Model.Element Element
		{
			get { return (Model.Element)this.GetValue(ElementProperty); }
			set { this.SetValue(ElementProperty, value); }
		}

		public static readonly DependencyProperty ElementProperty =
			DependencyProperty.Register(
				"Element",
				typeof(Model.Element),
				typeof(ElementPresenter),
				new PropertyMetadata(onElementPropertyChanged));

		private static void onElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ElementPresenter)d).UpdateElementBorderThickness(e.NewValue == null ?
				-1 :
				((Model.Element)e.NewValue).Index);
		}

		#endregion Element

	}
}
