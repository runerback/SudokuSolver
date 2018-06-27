using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SudokuFiller
{
    sealed class ElementBorderThicknessConverter : IValueConverter
    {
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int baseThickness = (int)(double)value; //unbox first
			int index = (int)parameter;

			switch (index)
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

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
