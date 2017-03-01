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
  class PickerConverter : IValueConverter
  {
    private DataService _dataService;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is EquipmentType)
      {
        var temp = (EquipmentType)value;
        return temp.Name;
      }
      if (value is ConductorSize)
      {
        var temp = (ConductorSize)value;
        return temp.Size;
      }
      if (value is Manufacturer)
      {
        var temp = (Manufacturer)value;
        return temp.Name;
      }
      if (value is BreakerStyle)
      {
        var temp = (BreakerStyle)value;
        return temp.Name;
      }
      if (value is TripUnitType)
      {
        var temp = (TripUnitType)value;
        return temp.Name;
      }
      if (value is LongTimeDelay)
      {
        var temp = (LongTimeDelay)value;
        return temp.Value;
      }
      if (value is ShortTimePickup)
      {
        var temp = (ShortTimePickup)value;
        return temp.Value;
      }
      if (value is ShortTimeDelay)
      {
        var temp = (ShortTimeDelay)value;
        return temp.Value;
      }
      if (value is Instantaneous)
      {
        var temp = (Instantaneous)value;
        return temp.Value;
      }
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      _dataService = new DataService();
      if (value != null)
      {
        if (value is EquipmentType)
          return (EquipmentType)value;
       if (value is ConductorSize)
         return (ConductorSize)value;
       if (value is Manufacturer)
          return (Manufacturer)value;
        if (value is BreakerStyle)
          return (BreakerStyle)value;
        if (value is TripUnitType)
          return (TripUnitType)value;
        if (value is LongTimeDelay)
        return (LongTimeDelay)value;
      if (value is ShortTimePickup)
        return (ShortTimePickup)value;
      if (value is ShortTimeDelay)
        return (ShortTimeDelay)value;
      if (value is Instantaneous)
        return (Instantaneous)value;

      switch (parameter as string)
      {
          case "EquipmentType":
           return _dataService.GetEquipmentTypes().Find(eq => eq.Name == value.ToString());
          case "ConductorSize":
            return _dataService.GetConductorSizes().Find(eq => eq.Size == value.ToString());
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
      }
      return -1;
    }
  }
}
