using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  class EnableConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool?)
      {
        if ((bool)value)
          return Color.FromHex("#3DCD58");
        return Color.Gray;
      }
      return Color.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if ((Color)value == Color.Gray)
        return false;
      return true;
    }
  }
}
