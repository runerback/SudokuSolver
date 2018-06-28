using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SudokuFiller
{
	sealed class ElementPresenter : Control
	{
        public ElementPresenter()
        {
			this.elementBorderThicknessUpdator = new BorderThicknessUpdator(this);
        }

		#region Layout Properties

		#region ElementBorderMargin

		public Thickness ElementBorderMargin
		{
			get { return (Thickness)this.GetValue(ElementBorderMarginProperty); }
			set { this.SetValue(ElementBorderMarginProperty, value); }
		}

		public static readonly DependencyProperty ElementBorderMarginProperty =
			DependencyProperty.Register(
				"ElementBorderMargin",
				typeof(Thickness),
				typeof(ElementPresenter));

		#endregion ElementBorderMargin

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
		}

		static readonly DependencyPropertyKey ElementBorderThicknessPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"ElementBorderThickness",
				typeof(Thickness),
				typeof(ElementPresenter),
				new PropertyMetadata());

		public static readonly DependencyProperty ElementBorderThicknessProperty =
			ElementBorderThicknessPropertyKey.DependencyProperty;

		private void SetElementBorderThickness(Thickness value)
		{
			SetValue(ElementBorderThicknessPropertyKey, value);
		}

		private readonly BorderThicknessUpdator elementBorderThicknessUpdator;

		#endregion ElementBorderThickness

		#endregion Layout Properties

        #region Element

        public ElementData Element
		{
			get { return (ElementData)this.GetValue(ElementProperty); }
			set { this.SetValue(ElementProperty, value); }
		}

		public static readonly DependencyProperty ElementProperty =
			DependencyProperty.Register(
				"Element",
				typeof(ElementData),
				typeof(ElementPresenter));

        #endregion Element

        #region IsInputMode

        public bool IsInputMode
		{
			get { return (bool)this.GetValue(IsInputModeProperty); }
			set { this.SetValue(IsInputModeProperty, value); }
		}

		public static readonly DependencyProperty IsInputModeProperty =
			DependencyProperty.Register(
				"IsInputMode",
				typeof(bool),
				typeof(ElementPresenter),
                new PropertyMetadata(true));
		
		#endregion IsInputMode

		class BorderThicknessUpdator : IDisposable
		{
			public BorderThicknessUpdator(ElementPresenter host)
			{
				if (host == null)
					throw new ArgumentNullException("host");
				this.host = host;

				DependencyPropertyDescriptor.FromProperty(
						ElementPresenter.ElementBorderWidthProperty,
						typeof(ElementPresenter))
						.AddValueChanged(host, UpdateNeeded);
				DependencyPropertyDescriptor.FromProperty(
					ElementPresenter.ElementProperty,
					typeof(ElementPresenter))
					.AddValueChanged(host, UpdateNeeded);
			}

			private readonly ElementPresenter host;

			private void UpdateNeeded(object sender, EventArgs e)
			{
				updateElementBorderThickness((ElementPresenter)sender);
			}

			private void updateElementBorderThickness(ElementPresenter host)
			{
				host.SetElementBorderThickness(calculateThickness(host));
			}

			private Thickness calculateThickness(ElementPresenter host)
			{
				var element = host.Element;
				if (element == null)
					return new Thickness();

				double baseThickness = host.ElementBorderWidth;
				switch (element.Index)
				{
					case 0:
					case 1:
					case 3:
					case 4:
						return new Thickness(0, 0, baseThickness, baseThickness);
					case 2:
					case 5:
						return new Thickness(0, 0, 0, baseThickness);
					case 6:
					case 7:
						return new Thickness(0, 0, baseThickness, 0);
					default:
						return new Thickness(0);
				}
			}

			public void Dispose()
			{
				DependencyPropertyDescriptor.FromProperty(
						ElementPresenter.ElementBorderWidthProperty,
						typeof(ElementPresenter))
						.RemoveValueChanged(host, UpdateNeeded);
				DependencyPropertyDescriptor.FromProperty(
					ElementPresenter.ElementProperty,
					typeof(ElementPresenter))
					.RemoveValueChanged(host, UpdateNeeded);
			}
		}
	}
}
