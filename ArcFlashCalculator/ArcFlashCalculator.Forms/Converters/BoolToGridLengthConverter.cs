using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  class BoolToGridLengthConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool)
      {
        if (parameter.ToString() == "HasCable" && (bool)value == false)
          return new GridLength(0, GridUnitType.Absolute);
        else if (parameter.ToString() == "HasCable" && (bool)value == true)
          return new GridLength(110, GridUnitType.Absolute);
        if (parameter.ToString() == "HasXfmr" && (bool)value == false)
          return new GridLength(0, GridUnitType.Absolute);
        else if (parameter.ToString() == "HasXfmr" && (bool)value == true)
          return new GridLength(110, GridUnitType.Absolute);
      }
      return new GridLength(0, GridUnitType.Absolute);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (parameter.ToString() == "HasCable" && (int)value == 0)
        return false;
      else if (parameter.ToString() == "HasCable" && (int)value == 110)
        return new GridLength(110, GridUnitType.Absolute);
      if (parameter.ToString() == "HasXfmr" && (int)value == 0)
        return false;
      else if (parameter.ToString() == "HasXfmr" && (int)value == 110)
        return true;
      return false;
    }
  }
}
