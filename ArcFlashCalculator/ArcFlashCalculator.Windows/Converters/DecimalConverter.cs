using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ArcFlashCalculator.WPF.Converters
{
  public class DecimalConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is decimal?)
        return value.ToString();
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      decimal dec;
      if (decimal.TryParse(value as string, out dec))
        return dec;
      return null;
    }
  }
}
