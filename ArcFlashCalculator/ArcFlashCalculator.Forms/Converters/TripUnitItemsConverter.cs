using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Services;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Converters
{
  class TripUnitItemsConverter:IValueConverter
  {
    private DataService _dataService;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return null;
      var list = (value as IList<TripUnitType>).Select((x) => x.Name).ToList();
      return list;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService=new DataService();
      if(value == null)
        return null;
      ;
      var item = _dataService.GetTripUnitTypes().Find(trip=>trip.Name==value.ToString());
      return item;
    }
  }
}
