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
	sealed class SudokuPresenter : Control
	{
		public SudokuPresenter()
		{
			this.sudokuBorderThicknessUpdator = new BorderThicknessUpdator(this);
		}
		
		#region Layout Properties

		#region SudokuBorderMargin

		public Thickness SudokuBorderMargin
		{
			get { return (Thickness)this.GetValue(SudokuBorderMarginProperty); }
			set { this.SetValue(SudokuBorderMarginProperty, value); }
		}

		public static readonly DependencyProperty SudokuBorderMarginProperty =
			DependencyProperty.Register(
				"SudokuBorderMargin",
				typeof(Thickness),
				typeof(SudokuPresenter));
		
		#endregion SudokuBorderMargin

		#region SudokuBorderBrush

		public Brush SudokuBorderBrush
		{
			get { return (Brush)this.GetValue(SudokuBorderBrushProperty); }
			set { this.SetValue(SudokuBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty SudokuBorderBrushProperty =
			DependencyProperty.Register(
                "SudokuBorderBrush",
				typeof(Brush),
				typeof(SudokuPresenter));
		
		#endregion SudokuBorderBrush

		#region SudokuBorderWidth

		public double SudokuBorderWidth
		{
			get { return (double)this.GetValue(SudokuBorderWidthProperty); }
			set { this.SetValue(SudokuBorderWidthProperty, value); }
		}

		public static readonly DependencyProperty SudokuBorderWidthProperty =
			DependencyProperty.Register(
				"SudokuBorderWidth",
				typeof(double),
				typeof(SudokuPresenter));
		
		#endregion SudokuBorderWidth

		#region SudokuBorderThickness
		public Thickness SudokuBorderThickness
		{
			get { return (Thickness)GetValue(SudokuBorderThicknessProperty); }
		}

		static readonly DependencyPropertyKey SudokuBorderThicknessPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"SudokuBorderThickness",
				typeof(Thickness),
				typeof(SudokuPresenter),
				new PropertyMetadata());

		public static readonly DependencyProperty SudokuBorderThicknessProperty =
			SudokuBorderThicknessPropertyKey.DependencyProperty;

		private void SetSudokuBorderThickness(Thickness value)
		{
			SetValue(SudokuBorderThicknessPropertyKey, value);
		}

		#endregion SudokuBorderThickness

		private readonly BorderThicknessUpdator sudokuBorderThicknessUpdator;

		#endregion Layout Properties
		
		#region Sudoku

		public SudokuData Sudoku
		{
			get { return (SudokuData)this.GetValue(SudokuProperty); }
			set { this.SetValue(SudokuProperty, value); }
		}

		public static readonly DependencyProperty SudokuProperty =
			DependencyProperty.Register(
				"Sudoku",
				typeof(SudokuData),
				typeof(SudokuPresenter));
		
		#endregion Sudoku
		
		#region Grid Properties

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
				typeof(SudokuPresenter));

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
				typeof(SudokuPresenter));

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
				typeof(SudokuPresenter));

		#endregion GridBorderWidth

		#endregion Grid Properties
		
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
				typeof(SudokuPresenter));

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
				typeof(SudokuPresenter));

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
				typeof(SudokuPresenter));

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
				typeof(SudokuPresenter));

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
                typeof(SudokuPresenter),
                new PropertyMetadata(true));

        #endregion IsInputMode

		#region KeyNavigation



		#endregion KeyNavigation

        class BorderThicknessUpdator : IDisposable
		{
			public BorderThicknessUpdator(SudokuPresenter host)
			{
				if (host == null)
					throw new ArgumentNullException("host");
				this.host = host;

				DependencyPropertyDescriptor.FromProperty(
					SudokuPresenter.SudokuBorderWidthProperty,
					typeof(SudokuPresenter))
					.AddValueChanged(host, UpdateNeeded);
				DependencyPropertyDescriptor.FromProperty(
					SudokuPresenter.SudokuProperty,
					typeof(SudokuPresenter))
					.AddValueChanged(host, UpdateNeeded);
			}

			private readonly SudokuPresenter host;

			private void UpdateNeeded(object sender, EventArgs e)
			{
				updateElementBorderThickness((SudokuPresenter)sender);
			}

			private void updateElementBorderThickness(SudokuPresenter host)
			{
				host.SetSudokuBorderThickness(calculateThickness(host));
			}

			private Thickness calculateThickness(SudokuPresenter host)
			{
				var sudoku = host.Sudoku;
				if (host.Sudoku == null)
					return new Thickness();
				return new Thickness(host.ElementBorderWidth);
			}

			public void Dispose()
			{
				DependencyPropertyDescriptor.FromProperty(
					SudokuPresenter.SudokuBorderWidthProperty,
					typeof(SudokuPresenter))
					.RemoveValueChanged(host, UpdateNeeded);
				DependencyPropertyDescriptor.FromProperty(
					SudokuPresenter.SudokuProperty,
					typeof(SudokuPresenter))
					.RemoveValueChanged(host, UpdateNeeded);
			}
		}
	}
}
