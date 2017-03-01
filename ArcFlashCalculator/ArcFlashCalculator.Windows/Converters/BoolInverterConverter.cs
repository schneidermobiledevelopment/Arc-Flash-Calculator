using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ArcFlashCalculator.WPF.Converters
{
  public class BoolInverterConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
      var test = (bool?)value;
      var result = bool.Parse((string)parameter);

      if (test == result)
      {
        return true;
      }

      return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
      var result = bool.Parse((string)parameter);
      return result;
    }

    #endregion
  }
}
