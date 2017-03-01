using System.Collections.Generic;
using ArcFlashCalculator.Core.Model;

namespace ArcFlashCalculator.Core.Interfaces
{
  public interface IDataService
  {
    List<EquipmentType> GetEquipmentTypes();
    List<EquipmentType> GetMediumToHighVoltageEquipmentTypes();
    List<Equipment> GetEquipments();
    List<ConductorSize> GetConductorSizes();
    List<ProtectiveEquipment> GetProtectiveEquipments();
    List<UnitOfMeasure> GetUnitOfMeasure();
    List<Manufacturer> GetManufacturers();
    List<BreakerStyle> GetBreakerStyles();
    List<TripUnitType> GetTripUnitTypes();
    List<LongTimeDelay> GetLongTimeDelays();
    List<ShortTimePickup> GetShortTimePickups();
    List<ShortTimeDelay> GetShortTimeDelays();
    List<Instantaneous> GetInstantaneous();
    List<WorkingDistance> GetWorkingDistances();
  }
}
