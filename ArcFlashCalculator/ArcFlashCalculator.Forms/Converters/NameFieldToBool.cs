using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  class NameFieldToBool:IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

      if (value is string && (string) parameter == value.ToString())
        return value;
      else
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value;
    }
  }
}
