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
  class EquipmentTypeConverter : IValueConverter
  {
    private DataService _dataService;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is EquipmentType)
      {
        var temp = (EquipmentType)value;
        return temp.Name;
      }
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService = new DataService();
      if (value == null)
        return -1;
      if (value is EquipmentType)
        return (EquipmentType)value;
      var item = _dataService.GetEquipmentTypes().Find(eq => eq.Name == value.ToString());
      return item;
      //if (value is EquipmentType)
      //  return (EquipmentType)value;
      //return null;
    }
  }
}
