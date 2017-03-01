using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Services;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  public  class ManufacterConverter : IValueConverter
  {
    private DataService _dataService;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is Manufacturer)
      {
        var temp = (Manufacturer)value;
        return temp.Name;
      }
      return value;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService = new DataService();
      if (value == null)
        return -1;
      if (value is Manufacturer)
        return (Manufacturer)value;
      var item = _dataService.GetManufacturers().Find(m => m.Name == value.ToString());
      return item;
    }
  }
}
