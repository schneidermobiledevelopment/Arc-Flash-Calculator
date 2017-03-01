using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ArcFlashCalculator.WPF.Converters
{
  class InverseBoolToVisibilityConverter: IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
      var test = (bool)value;
      if (!test)
      {
        return Visibility.Visible;
      }
      else return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
      return false;
    }

    #endregion
  }
}
