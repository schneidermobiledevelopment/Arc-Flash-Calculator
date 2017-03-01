using ArcFlashCalculator.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.ObjectModel;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Services;

namespace ArcFlashCalculator.Forms.Converters
{
  class ConductorSizeItemsConverter : IValueConverter
  {
    private DataService _dataService;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return null;
      var list = (value as ICollection<ConductorSize>).Select((x) => x.Size);
      return list;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService = new DataService();
      if (value ==  null)
        return null;
      var item = _dataService.GetConductorSizes().Find(cond => cond.Size == value.ToString());
      return item;
    }
  }
}