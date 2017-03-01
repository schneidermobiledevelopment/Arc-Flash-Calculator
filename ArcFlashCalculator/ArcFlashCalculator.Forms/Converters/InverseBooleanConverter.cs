using System;
using System.Globalization;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  class InverseBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool?)
        return !(bool)value;
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool)
        return !(bool)value;
      return null;
    }
  }
}
