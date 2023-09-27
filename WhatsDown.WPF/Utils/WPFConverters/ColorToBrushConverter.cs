using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WhatsDown.WPF.Utils.WPFConverters;

class ColorToBrushConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Color clr)
			return new SolidColorBrush(clr);
		throw new InvalidCastException();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}