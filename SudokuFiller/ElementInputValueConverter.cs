using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SudokuFiller
{
	sealed class ElementInputValueConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;
			return value.ToString();
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string input = value as string;
			if (string.IsNullOrEmpty(input))
				return null;
			int elementValue = 0;
			int.TryParse(input, out elementValue);
			if (elementValue <= 0)
				return null;
			else if (elementValue > 9)
				return elementValue / 10;
			return elementValue;
		}
	}
}
