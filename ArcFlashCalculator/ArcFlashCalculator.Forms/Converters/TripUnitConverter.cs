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
  class TripUnitConverter:IValueConverter
  {
    private DataService _dataService;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is TripUnitType)
      {
        var temp = (TripUnitType) value;
        return temp.Name;
      }
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService= new DataService();
      if (value == null)
        return null;
      if (value is TripUnitType)
        return (TripUnitType)value;
      var item = _dataService.GetTripUnitTypes().Find(trip => trip.Name == value.ToString());
      return item;
    }
  }
}
