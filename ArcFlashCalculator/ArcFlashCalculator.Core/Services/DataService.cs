using System.Collections.Generic;
using System.Linq;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;

namespace ArcFlashCalculator.Core.Services
{
  public class DataService : IDataService
  {
    public List<EquipmentType> GetEquipmentTypes()
    {
      return new List<EquipmentType>
        {
            new EquipmentType { Id = 0, Name = "Open Air", IsLowVoltageOnly = false },
            new EquipmentType { Id = 1, Name = "Cable", IsLowVoltageOnly = false },
            new EquipmentType { Id = 2, Name = "Low Voltage Switchgear", IsLowVoltageOnly = true },
            new EquipmentType { Id = 3, Name = "Medium Voltage Switchgear", IsLowVoltageOnly = false },
            new EquipmentType { Id = 4, Name = "Panel, Switchboard, or UPS", IsLowVoltageOnly = true },
            new EquipmentType { Id = 5, Name = "Motor Control Center", IsLowVoltageOnly = true }
        };
    }

    public List<EquipmentType> GetMediumToHighVoltageEquipmentTypes()
    {
      return GetEquipmentTypes().Where(o => !o.IsLowVoltageOnly).ToList();
    }

    public List<WorkingDistance> GetWorkingDistances()
    {
      return new List<WorkingDistance>
        {
            new WorkingDistance { Id = 0, Name = "18\" - Standard for MCCs/Panelboards", Value = 18 },
            new WorkingDistance { Id = 1, Name = "24\" - Standard for Low Voltage Switchgear", Value = 24 },
            new WorkingDistance { Id = 2, Name = "36\" - Standard for Medium Voltage Equipment", Value = 36}
        };
    }

    public List<ConductorSize> GetConductorSizes()
    {
      return new List<ConductorSize>
        {
            new ConductorSize {Id = 0,  OhmsPerThousandFeet = 0.814m,  Size = "#8 and smaller"},
            new ConductorSize {Id = 1,  OhmsPerThousandFeet = 0.515m,  Size = "#6"},
            new ConductorSize {Id = 2,  OhmsPerThousandFeet = 0.327m,  Size = "#4"},
            new ConductorSize {Id = 3,  OhmsPerThousandFeet = 0.21m,   Size = "#2"},
            new ConductorSize {Id = 4,  OhmsPerThousandFeet = 0.17m,   Size = "#1"},
            new ConductorSize {Id = 5,  OhmsPerThousandFeet = 0.139m,  Size = "#1/0"},
            new ConductorSize {Id = 6,  OhmsPerThousandFeet = 0.115m,  Size = "#2/0"},
            new ConductorSize {Id = 7,  OhmsPerThousandFeet = 0.0958m, Size = "#3/0"},
            new ConductorSize {Id = 8,  OhmsPerThousandFeet = 0.081m,  Size = "#4/0"},
            new ConductorSize {Id = 9,  OhmsPerThousandFeet = 0.0742m, Size = "250"},
            new ConductorSize {Id = 10, OhmsPerThousandFeet = 0.0617m, Size = "350"},
            new ConductorSize {Id = 11, OhmsPerThousandFeet = 0.0606m, Size = "400"},
            new ConductorSize {Id = 12, OhmsPerThousandFeet = 0.0551m, Size = "500"},
            new ConductorSize {Id = 13, OhmsPerThousandFeet = 0.053m,  Size = "600"},
            new ConductorSize {Id = 14, OhmsPerThousandFeet = 0.0495m, Size = "750"}
        };
    }

    public List<Equipment> GetEquipments()
    {
      return new List<Equipment>
            {
                // 208V-1kV
                new Equipment {Id = 0,  EquipmentTypeId = 0 , Voltage_Min = 208m, Voltage_Max = 1000m, Typical_Bus_Gap = 40, Distance_X_Fact = 2m, WorkingDistanceId=0},
                new Equipment {Id = 1,  EquipmentTypeId = 1 , Voltage_Min = 208m, Voltage_Max = 1000m, Typical_Bus_Gap = 13, Distance_X_Fact = 2m, WorkingDistanceId=0},
                new Equipment {Id = 2,  EquipmentTypeId = 2 , Voltage_Min = 208m, Voltage_Max = 1000m, Typical_Bus_Gap = 32, Distance_X_Fact = 1.473m, WorkingDistanceId=1},
                new Equipment {Id = 3,  EquipmentTypeId = 4 , Voltage_Min = 208m, Voltage_Max = 1000m, Typical_Bus_Gap = 25, Distance_X_Fact = 1.641m, WorkingDistanceId=0},
                new Equipment {Id = 4,  EquipmentTypeId = 5 , Voltage_Min = 208m, Voltage_Max = 1000m, Typical_Bus_Gap = 25, Distance_X_Fact = 1.641m, WorkingDistanceId=0},

                // >1kV-5kV
                new Equipment {Id = 5,  EquipmentTypeId = 0 , Voltage_Min = 1001m, Voltage_Max = 5000m, Typical_Bus_Gap = 102, Distance_X_Fact = 2m, WorkingDistanceId=2},
                new Equipment {Id = 6,  EquipmentTypeId = 1 , Voltage_Min = 1001m, Voltage_Max = 5000m, Typical_Bus_Gap = 13, Distance_X_Fact = 2m, WorkingDistanceId=2},
                new Equipment {Id = 7,  EquipmentTypeId = 3 , Voltage_Min = 1001m, Voltage_Max = 5000m, Typical_Bus_Gap = 102,  Distance_X_Fact = 0.973m, WorkingDistanceId=2},

                // >5kV-15kV
                new Equipment {Id = 8,  EquipmentTypeId = 0 , Voltage_Min = 5001m, Voltage_Max = 15000m, Typical_Bus_Gap = 153, Distance_X_Fact = 2m, WorkingDistanceId=2},
                new Equipment {Id = 9,  EquipmentTypeId = 1 , Voltage_Min = 5001m, Voltage_Max = 15000m, Typical_Bus_Gap = 13, Distance_X_Fact = 2m, WorkingDistanceId=2},
                new Equipment {Id = 10, EquipmentTypeId = 3 , Voltage_Min = 5001m, Voltage_Max = 15000m, Typical_Bus_Gap = 153,  Distance_X_Fact = 0.973m, WorkingDistanceId=2},

                // >15kv
                new Equipment {Id = 11, EquipmentTypeId = 0 , Voltage_Min = 15001m, Voltage_Max = 36000m, Typical_Bus_Gap = 0, Distance_X_Fact = 0, WorkingDistanceId=2}, 
                new Equipment {Id = 12, EquipmentTypeId = 1 , Voltage_Min = 15001m, Voltage_Max = 36000m, Typical_Bus_Gap = 0, Distance_X_Fact = 0, WorkingDistanceId=2},
                new Equipment {Id = 13, EquipmentTypeId = 3 , Voltage_Min = 15001m, Voltage_Max = 36000m, Typical_Bus_Gap = 0,  Distance_X_Fact = 0, WorkingDistanceId=2}

            };
    }

    public List<ProtectiveEquipment> GetProtectiveEquipments()
    {
      return new List<ProtectiveEquipment>
            {
                new ProtectiveEquipment { Id = 0, RiskCategory = "0", Rating_Min = 0m , Rating_Max = 1.2m },
                //new ProtectiveEquipment { Id = 1, RiskCategory = "1", Rating_Min = 1.2m , Rating_Max = 4m },
                new ProtectiveEquipment { Id = 2, RiskCategory = "2", Rating_Min = 1.2m , Rating_Max = 8m },
                //new ProtectiveEquipment { Id = 3, RiskCategory = "3", Rating_Min = 8m, Rating_Max = 25m },
                new ProtectiveEquipment { Id = 4, RiskCategory = "4", Rating_Min = 8m, Rating_Max = 40m },
                new ProtectiveEquipment { Id = 5, RiskCategory ="Danger", Rating_Min = 40m, Rating_Max = decimal.MaxValue},
                };
    }

    public List<UnitOfMeasure> GetUnitOfMeasure()
    {
      return new List<UnitOfMeasure>
            {
                new UnitOfMeasure { Id = 0, Name = "Feet" },
                new UnitOfMeasure { Id = 1, Name = "Inches" },
                };
    }

    public List<LongTimeDelay> GetLongTimeDelay()
    {
      return new List<LongTimeDelay>
      {
        new LongTimeDelay {Id = 0 , Value = 0.5m},
        new LongTimeDelay {Id = 1 , Value = 1m},
        new LongTimeDelay {Id = 2 , Value = 2m},
        new LongTimeDelay {Id = 3 , Value = 4m},
        new LongTimeDelay {Id = 4 , Value = 8m},
        new LongTimeDelay {Id = 5 , Value = 12m},
        new LongTimeDelay {Id = 6 , Value = 16m},
        new LongTimeDelay {Id = 7 , Value = 20m},
        new LongTimeDelay {Id = 8 , Value = 24m},
      };
    }


    public List<Manufacturer> GetManufacturers()
    {
      return new List<Manufacturer>
      {
        new Manufacturer {Id = 0 , Name = "Square D/Schneider Electric"},
        //new Manufacturer {Id = 0 , Name = "GE"}
      };
    }

    public List<BreakerStyle> GetBreakerStyles()
    {
      return new List<BreakerStyle>
      {
        new BreakerStyle {Id = 0 , ManufacturerId = 0, Name = "Masterpact NW/NT"},
        //new BreakerStyle {Id = 1 , ManufacturerId = 1, Name = "AK"},
      };
    }

    public List<TripUnitType> GetTripUnitTypes()
    {
      return new List<TripUnitType>
      {
        new TripUnitType {Id = 0, BreakerStyleId = 0, Name = "Micrologic"},
        //new TripUnitType {Id = 1, BreakerStyleId = 1, Name = "Micro Versa"},
        //new TripUnitType {Id = 2, BreakerStyleId = 0, Name = "5.0A"},
        //new TripUnitType {Id = 3, BreakerStyleId = 0, Name = "6.0A"}
      };
    }

    public List<LongTimeDelay> GetLongTimeDelays()
    {
      return new List<LongTimeDelay>()
      {
        new LongTimeDelay() {Id = 0, TripUnitTypeId = 0, Value = .5m},
        new LongTimeDelay() {Id = 1, TripUnitTypeId = 0, Value = 1},
        new LongTimeDelay() {Id = 2, TripUnitTypeId = 0, Value = 2},
        new LongTimeDelay() {Id = 3, TripUnitTypeId = 0, Value = 4},
        new LongTimeDelay() {Id = 4, TripUnitTypeId = 0, Value = 8},
        new LongTimeDelay() {Id = 5, TripUnitTypeId = 0, Value = 12},
        new LongTimeDelay() {Id = 6, TripUnitTypeId = 0, Value = 16},
        new LongTimeDelay() {Id = 7, TripUnitTypeId = 0, Value = 20},
        new LongTimeDelay() {Id = 8, TripUnitTypeId = 0, Value = 24}
      };
    }

    public List<ShortTimePickup> GetShortTimePickups()
    {
      return new List<ShortTimePickup>
      {
        new ShortTimePickup {Id = 0, TripUnitTypeId = 0 , Value = "N/A"},
        new ShortTimePickup {Id = 1, TripUnitTypeId = 0 , Value = "1.5"},
        new ShortTimePickup {Id = 2, TripUnitTypeId = 0 , Value = "2"},
        new ShortTimePickup {Id = 3, TripUnitTypeId = 0 , Value = "2.5"},
        new ShortTimePickup {Id = 4, TripUnitTypeId = 0 , Value = "3"},
        new ShortTimePickup {Id = 5, TripUnitTypeId = 0 , Value = "4"},
        new ShortTimePickup {Id = 6, TripUnitTypeId = 0 , Value = "5"},
        new ShortTimePickup {Id = 7, TripUnitTypeId = 0 , Value = "6"},
        new ShortTimePickup {Id = 8, TripUnitTypeId = 0 , Value = "8"},
        new ShortTimePickup {Id = 9, TripUnitTypeId = 0 , Value = "10"}
      };
    }


    public List<ShortTimeDelay> GetShortTimeDelays()
    {
      return new List<ShortTimeDelay>
      {
        new ShortTimeDelay {Id = 0, TripUnitTypeId = 0 , Value = "N/A"},
        new ShortTimeDelay {Id = 1, TripUnitTypeId = 0 , Value = "0.1 on"},
        new ShortTimeDelay {Id = 2, TripUnitTypeId = 0 , Value = "0.2 on"},
        new ShortTimeDelay {Id = 3, TripUnitTypeId = 0 , Value = "0.3 on"},
        new ShortTimeDelay {Id = 4, TripUnitTypeId = 0 , Value = "0.4 on"},
        new ShortTimeDelay {Id = 5, TripUnitTypeId = 0 , Value = "0 off"},
        new ShortTimeDelay {Id = 6, TripUnitTypeId = 0 , Value = "0.1 off"},
        new ShortTimeDelay {Id = 7, TripUnitTypeId = 0 , Value = "0.2 off"},
        new ShortTimeDelay {Id = 8, TripUnitTypeId = 0 , Value = "0.3 off"},
        new ShortTimeDelay {Id = 9, TripUnitTypeId = 0 , Value = "0.4 off"}
      };
    }

    public List<Instantaneous> GetInstantaneous()
    {
      return new List<Instantaneous>
      {
        new Instantaneous {Id = 0, TripUnitTypeId = 0 , Value = "N/A"},
        new Instantaneous {Id = 1, TripUnitTypeId = 0 , Value = "2"},
        new Instantaneous {Id = 2, TripUnitTypeId = 0 , Value = "3"},
        new Instantaneous {Id = 3, TripUnitTypeId = 0 , Value = "4"},
        new Instantaneous {Id = 4, TripUnitTypeId = 0 , Value = "6"},
        new Instantaneous {Id = 5, TripUnitTypeId = 0 , Value = "8"},
        new Instantaneous {Id = 6, TripUnitTypeId = 0 , Value = "10"},
        new Instantaneous {Id = 7, TripUnitTypeId = 0 , Value = "12"},
        new Instantaneous {Id = 8, TripUnitTypeId = 0 , Value = "15"},
        new Instantaneous {Id = 9, TripUnitTypeId = 0 , Value = "off"}
      };
    }
  }
}
