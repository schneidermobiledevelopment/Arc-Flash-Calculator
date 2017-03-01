using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  class ConductorSizeConverter : IValueConverter
  {
    private DataService _dataService;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is ConductorSize)
      {
        var temp = (ConductorSize)value;
        return temp.Size;
      }
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService = new DataService();
      if (value == null)
        return null;
      if (value is ConductorSize)
        return (ConductorSize)value;
      var item = _dataService.GetConductorSizes().Find(eq => eq.Size == value.ToString());
      return item;
    }
  }
}
