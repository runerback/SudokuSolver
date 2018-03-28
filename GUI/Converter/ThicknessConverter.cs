using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SudokuSolver.GUI.Converter
{
	internal sealed class ThicknessConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return default(Thickness);

			var thicknessParameter = parameter as ThicknessConverterParameter;
			if (thicknessParameter == null)
				return default(Thickness);

			return thicknessParameter.GetThickness((double)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
