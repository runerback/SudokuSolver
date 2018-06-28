using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuFiller
{
	sealed class GridPresenter : Control
	{
		public GridPresenter()
		{
			this.gridBorderThicknessUpdator = new BorderThicknessUpdator(this);
		}
		
		#region Layout Properties

		#region GridBorderMargin

		public Thickness GridBorderMargin
		{
			get { return (Thickness)this.GetValue(GridBorderMarginProperty); }
			set { this.SetValue(GridBorderMarginProperty, value); }
		}

		public static readonly DependencyProperty GridBorderMarginProperty =
			DependencyProperty.Register(
				"GridBorderMargin",
				typeof(Thickness),
				typeof(GridPresenter));

		#endregion GridBorderMargin

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
			get { return (Thickness)GetValue(GridBorderThicknessProperty); }
		}

		static readonly DependencyPropertyKey GridBorderThicknessPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"GridBorderThickness",
				typeof(Thickness),
				typeof(GridPresenter),
				new PropertyMetadata());

		public static readonly DependencyProperty GridBorderThicknessProperty =
			GridBorderThicknessPropertyKey.DependencyProperty;

		private void SetGridBorderThickness(Thickness value)
		{
			SetValue(GridBorderThicknessPropertyKey, value);
		}
		
		private readonly BorderThicknessUpdator gridBorderThicknessUpdator;

		#endregion GridBorderThickess

		#endregion Layout Properties

		#region Grid

		public GridData Grid
		{
			get { return (GridData)this.GetValue(GridProperty); }
			set { this.SetValue(GridProperty, value); }
		}

		public static readonly DependencyProperty GridProperty =
			DependencyProperty.Register(
				"Grid",
				typeof(GridData),
				typeof(GridPresenter));

		#endregion Grid

		#region Element Properties

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
				typeof(GridPresenter));

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

        #endregion Element Properties

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
                typeof(GridPresenter),
                new PropertyMetadata(true));

        #endregion IsInputMode

        class BorderThicknessUpdator : IDisposable
		{
			public BorderThicknessUpdator(GridPresenter host)
			{
				if (host == null)
					throw new ArgumentNullException("host");
				this.host = host;

				DependencyPropertyDescriptor.FromProperty(
					GridPresenter.GridBorderWidthProperty,
					typeof(GridPresenter))
					.AddValueChanged(host, UpdateNeeded);
				DependencyPropertyDescriptor.FromProperty(
					GridPresenter.GridProperty,
					typeof(GridPresenter))
					.AddValueChanged(host, UpdateNeeded);
			}

			private readonly GridPresenter host;

			private void UpdateNeeded(object sender, EventArgs e)
			{
				updateElementBorderThickness((GridPresenter)sender);
			}

			private void updateElementBorderThickness(GridPresenter host)
			{
				host.SetGridBorderThickness(calculateThickness(host));
			}

			private Thickness calculateThickness(GridPresenter host)
			{
				var grid = host.Grid;
				if (grid == null)
					return new Thickness();

				double baseThickness = host.GridBorderWidth;
				switch (grid.Index)
				{
					case 0:
					case 1:
					case 3:
					case 4:
						return new Thickness(baseThickness, baseThickness, 0, 0);
					case 2:
					case 5:
						return new Thickness(baseThickness, baseThickness, baseThickness, 0);
					case 6:
					case 7:
						return new Thickness(baseThickness, baseThickness, 0, baseThickness);
					case 8:
						return new Thickness(baseThickness);
					default:
						return new Thickness(0);
				}
			}

			public void Dispose()
			{
				DependencyPropertyDescriptor.FromProperty(
					GridPresenter.GridBorderWidthProperty,
					typeof(GridPresenter))
					.RemoveValueChanged(host, UpdateNeeded);
				DependencyPropertyDescriptor.FromProperty(
					GridPresenter.GridProperty,
					typeof(GridPresenter))
					.RemoveValueChanged(host, UpdateNeeded);
			}
		}
	}
}
