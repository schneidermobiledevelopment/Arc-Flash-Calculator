using System;
using System.Globalization;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  class IntConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is Int32?)
        return value;
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Int32 dec;
      if (value != null)
      {
        if (value.ToString() == ".")
          return 0;
      }
      
      if (Int32.TryParse(value as string, out dec))
        return dec;
      return null;
    }
  }
}
