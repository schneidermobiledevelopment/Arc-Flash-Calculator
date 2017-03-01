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
  class PickerItemsConverter:IValueConverter
  {
    private DataService _dataService;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
      switch (parameter as string)
      {
          case "EquipmentType":
            return (value as ICollection<EquipmentType>).Select((x) => x.Name).ToList();
          case "ConductorSize":
            return (value as ICollection<ConductorSize>).Select((x) => x.Size).ToList();
          case "Manufacturer":
          return (value as IList<Manufacturer>).Select((x) => x.Name).ToList();
          case "BreakerStyle":
            return (value as IList<BreakerStyle>).Select((x) => x.Name).ToList();
          case "TripUnitType":
            return (value as IList<TripUnitType>).Select((x) => x.Name).ToList();
          case "LongTimeDelay":
            return (value as IList<LongTimeDelay>).Select((o) => o.Value).ToList();
          case "ShortTimePickup":
          return (value as IList<ShortTimePickup>).Select((o) => o.Value).ToList();
          case "ShortTimeDelay":
            return (value as IList<ShortTimeDelay>).Select((o) => o.Value).ToList();
          case "Instantaneous":
            return (value as IList<Instantaneous>).Select((o) => o.Value).ToList();
        }
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService = new DataService();
      if (value != null)
        switch (parameter as string)
        {
          case "EquipmentType":
            return _dataService.GetEquipmentTypes().Find(eq => eq.Name == value.ToString());
          case "ConductorSize":
            return _dataService.GetConductorSizes().Find(cond => cond.Size == value.ToString());
          case "Manufacturer":
            return _dataService.GetManufacturers().Find(m => m.Name == value.ToString());
          case "BreakerStyle":
            return _dataService.GetBreakerStyles().Find(m => m.Name == value.ToString());
          case "TripUnitType":
            return _dataService.GetTripUnitTypes().Find(trip => trip.Name == value.ToString());
          case "LongTimeDelay":
            return _dataService.GetLongTimeDelays().Find(o => o.Value.ToString() == value.ToString());
          case "ShortTimePickup":
            return _dataService.GetShortTimePickups().Find(o => o.Value.ToString() == value.ToString());
          case "ShortTimeDelay":
            return _dataService.GetShortTimeDelays().Find(o => o.Value.ToString() == value.ToString());
          case "Instantaneous":
            return _dataService.GetInstantaneous().Find(o => o.Value.ToString() == value.ToString());
        }

      return null;
    }
  }
}
