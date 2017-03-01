using System;
using System.Linq;
using System.Linq.Expressions;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;

namespace ArcFlashCalculator.Core.Services
{
  public class CalculatorService : ICalculatorService
  {
    private const int STANDARD_WORKING_DISTANCE_DC_18_INCHES = 18;
    private readonly IDataService _dataService;

    #region Constructor
    public CalculatorService(IDataService dataService, ICalculationOutput calculationOutput)
    {
      _dataService = dataService;
      _arcFlashOut = calculationOutput as CalculationOutput;
    }
    //public CalculatorService(CalculationInput input)
    //{
    //  ArcFlashIn = input;
    //  if (ArcFlashIn.PrimaryVoltage == 0)
    //  {
    //    ArcFlashIn.PrimaryVoltage = ArcFlashIn.NominalVoltage.Value;
    //  }
    //  if (ArcFlashIn.IsAlternatingCurrent.Value)
    //    Equipment = _dataService.GetEquipments().Find(eq => (eq.Voltage_Min <= ArcFlashIn.NominalVoltage && eq.Voltage_Max >= ArcFlashIn.NominalVoltage) && eq.Equipment_TypeID == ArcFlashIn.EquipmentTypeId);
    //}
    #endregion
    #region Properties

    private Equipment _equipment;
    private Equipment Equipment
    {
      get { return _equipment; }
      set { _equipment = value; }
    }

    private CalculationInput _arcFlashIn;
    private CalculationInput ArcFlashIn
    {
      get { return _arcFlashIn; }
      set { _arcFlashIn = value; }
    }

    private CalculationOutput _arcFlashOut;
    private CalculationOutput ArcFlashOut
    {
      get { return _arcFlashOut; }
      set { _arcFlashOut = value; }
    }
    #endregion


    #region ACCalculation
    public decimal CalcBoltedFaultCurrentAC(CalculationInput calculationInput)
    {
      ArcFlashIn = calculationInput;
      var sb = 100;
      var vb1 = ArcFlashIn.PrimaryVoltage.HasValue ? ArcFlashIn.PrimaryVoltage.Value : ArcFlashIn.NominalVoltage.Value;
      var vb2 = (ArcFlashIn.HasTransformer.HasValue && ArcFlashIn.HasTransformer.Value) ? ArcFlashIn.NominalVoltage.Value : vb1;
      decimal Ib1 = (decimal)(sb * Math.Pow(10, 6)) / (Decimal.Multiply((decimal)Math.Sqrt(3), vb1));
      decimal Ib2 = (decimal)(sb * Math.Pow(10, 6)) / (Decimal.Multiply((decimal)Math.Sqrt(3), vb2));
      decimal zb1 = decimal.Divide(vb1, (decimal)Math.Sqrt(3)) * (1 / Ib1);
      decimal zb2 = decimal.Divide(vb2, (decimal)Math.Sqrt(3)) * (1 / Ib2);

      decimal utilZ = 1 / (ArcFlashIn.SourceFaultCurrent.Value / Ib1);
      decimal xfmrZ;
      decimal cblZ;
      if (ArcFlashIn.HasTransformer.Value && ArcFlashIn.XfmrImpedance.HasValue && ArcFlashIn.XfmrKVA.HasValue && ArcFlashIn.XfmrKVA.Value > 0)
      {
        xfmrZ = (ArcFlashIn.XfmrImpedance.Value / 100) * (sb * 1000m) / ArcFlashIn.XfmrKVA.Value;
      }
      else
        xfmrZ = 0;
      if (ArcFlashIn.HasCable.Value)
      {
        var conductor = _dataService.GetConductorSizes().First(o => o.Id == ArcFlashIn.ConductorSizeId);
        cblZ = ((ArcFlashIn.ConductorLength.Value * conductor.OhmsPerThousandFeet / 1000) / ArcFlashIn.ConductorPerPhase.Value) / zb2;
      }
      else
        cblZ = 0;

      var BFC = Ib1 * (1 / (utilZ + xfmrZ + cblZ)) * (vb1 / vb2) / 1000;
      return BFC;
    }

    public decimal? CalculateArcFaultCurrentAC(CalculationInput calculationInput)
    {
      ArcFlashIn = calculationInput;

      if (ArcFlashIn.NominalVoltage.HasValue && ArcFlashIn.NominalVoltage.Value < 208) return null;
      if (!ArcFlashIn.IsAlternatingCurrent.HasValue) return null;
      if (!ArcFlashIn.NominalVoltage.HasValue) return null;
      if (!ArcFlashIn.SourceFaultCurrent.HasValue) return null;
      if (!ArcFlashIn.HasTransformer.HasValue) return null;
      if (ArcFlashIn.HasTransformer.Value && !ArcFlashIn.XfmrKVA.HasValue) return null;
      if (ArcFlashIn.HasTransformer.Value && !ArcFlashIn.XfmrImpedance.HasValue) return null;
      if (!ArcFlashIn.HasCable.HasValue) return null;
      if (ArcFlashIn.HasCable.Value && !ArcFlashIn.ConductorLength.HasValue) return null;
      if (ArcFlashIn.HasCable.Value && !ArcFlashIn.ConductorPerPhase.HasValue) return null;

      if (ArcFlashIn.IsAlternatingCurrent.Value && ArcFlashIn.NominalVoltage < 15000)
        Equipment = GetEquipment();

      //do bolted current calculation
      ArcFlashOut.BoltedFaultCurrent = CalcBoltedFaultCurrentAC(ArcFlashIn);

      decimal pow = 0m;
      double boltedFaultCurrent = (double)ArcFlashOut.BoltedFaultCurrent;
      decimal nominalVoltage = ArcFlashIn.NominalVoltage.Value;
      decimal ArcValue = ArcFlashIn.IsOpenAir != null && (ArcFlashIn.IsOpenAir.Value) ? -0.153m : -0.097m;
      if (ArcFlashIn.NominalVoltage < 1000m)
      {
          pow = ArcValue + 0.662m * (decimal)Math.Log10((double)ArcFlashOut.BoltedFaultCurrent) + 0.0966m * (nominalVoltage / 1000m) + 0.000526m * Equipment.Typical_Bus_Gap + 0.5588m * (nominalVoltage / 1000m) * (decimal)Math.Log10(boltedFaultCurrent) - 0.00304m * Equipment.Typical_Bus_Gap * (decimal)Math.Log10(boltedFaultCurrent);
      }
      else
        pow = (0.00402m + 0.983m * (decimal)Math.Log10((double)ArcFlashOut.BoltedFaultCurrent));
      var AFC = Convert.ToDecimal(Math.Pow(10, (Convert.ToDouble(pow))));
      return AFC;
    }

    private Equipment GetEquipment()
    {
      return _dataService.GetEquipments().Find(eq => (eq.Voltage_Min <= ArcFlashIn.NominalVoltage && eq.Voltage_Max >= ArcFlashIn.NominalVoltage) && eq.EquipmentTypeId == ArcFlashIn.EquipmentTypeId);
    }

    private decimal GetWorkingDistance(int workingDistanceId)
    {
      return _dataService.GetWorkingDistances().Find(o => o.Id == workingDistanceId).Value;
    }

    public decimal CalckAAC()
    {
      decimal KA = ArcFlashOut.ArcFlashBoundary / ArcFlashOut.BoltedFaultCurrent.Value * 100m;
      return KA;
    }

    public decimal CalcXAC(decimal? estimatedArcFaultCurrent)
    {
      decimal ArcValue = (ArcFlashIn.IsOpenAir.Value) ? -0.792m : -0.555m;
      decimal groundingValue = (ArcFlashIn.IsSolidGround.HasValue && ArcFlashIn.IsSolidGround.Value) ? -0.113m : 0m;
      var XAC = Convert.ToDecimal(Math.Pow(10, (double)((ArcValue + groundingValue) + 1.081m * (decimal)Math.Log10((double)estimatedArcFaultCurrent.Value) + 0.0011m * Equipment.Typical_Bus_Gap)));
      return XAC;
    }

    public decimal? CalcIncidentEnergyAC(decimal workingDistance, decimal? estimatedArcFaultCurrent)
    {
      if (!ArcFlashIn.ArcDurationValue.HasValue) return null;
      var IE = 0m;
      if (ArcFlashIn.NominalVoltage > 15000)
      {
        decimal voltageValue1 = (ArcFlashIn.NominalVoltage.Value > 15000)
          ? 2.142m * (decimal)Math.Pow(10, 6) * (ArcFlashIn.NominalVoltage.Value / 1000m) *
            ArcFlashOut.BoltedFaultCurrent.Value *
            (ArcFlashIn.ArcDurationValue.Value / (decimal)Math.Pow((double)(workingDistance * 25.4m), 2))
          : 4.184m;
        IE = (1m / 4.184m) * voltageValue1;
      }
      else
      {
        var XAC = CalcXAC(estimatedArcFaultCurrent);
        decimal voltageValue2 = (ArcFlashIn.NominalVoltage > 1000) ? 1m : 1.5m;
        IE = (1m / 4.184m) * 4.184m * voltageValue2 * XAC * (ArcFlashIn.ArcDurationValue.Value / 0.2m) * (((decimal)Math.Pow(610, (double)Equipment.Distance_X_Fact) / (decimal)Math.Pow((double)(25.4m * workingDistance), (double)Equipment.Distance_X_Fact)));
      }
      return IE;
    }

    public decimal CalcFlashHazardBoundaryAC(decimal? estimatedArcFaultCurrent)
    {
      decimal voltageValue2 = (ArcFlashIn.NominalVoltage > 1000) ? 1m : 1.5m;
      decimal voltageValue1;
      if (ArcFlashIn.NominalVoltage > 15000)
      {
        voltageValue1 = (decimal)Math.Sqrt((double)(2.142m * (decimal)Math.Pow(10, 6) * (ArcFlashIn.NominalVoltage / 1000m) * ArcFlashOut.BoltedFaultCurrent * (ArcFlashIn.ArcDurationValue.Value / 5m)));
      }
      else
      {
        var XAC = CalcXAC(estimatedArcFaultCurrent);
        voltageValue1 = (decimal)Math.Pow((double)(4.184m * voltageValue2 * XAC * (ArcFlashIn.ArcDurationValue.Value / 0.2m) * (((decimal)(Math.Pow(610, (double)Equipment.Distance_X_Fact)) / 5m))), (double)(1m / Equipment.Distance_X_Fact));
      }
      var FHB = (1m / 12m) * (1m / 25.4m) * voltageValue1;
      return FHB;
    }

    public decimal? CalculateMultipleSensorRatingAC(CalculationInput calculationInput, decimal? estimatedArcFaultCurrent)
    {
      if (calculationInput.SensorRating < 15 || calculationInput.SensorRating > 6000) return null;
      if (!calculationInput.SensorRating.HasValue) return null;
      if (!estimatedArcFaultCurrent.HasValue) return null;
      var MSR = (calculationInput.SensorRating == 0 || !calculationInput.SensorRating.HasValue) ? 0m : (1000m * estimatedArcFaultCurrent) / calculationInput.SensorRating;

      return MSR.Value;
    }

    public string RiskCategoryAC(CalculationOutput calculationOutput)
    {
      var risk = _dataService.GetProtectiveEquipments().Find(ppe => Math.Round(calculationOutput.IncidentEnergy, 2) >= ppe.Rating_Min && Math.Round(calculationOutput.IncidentEnergy, 2) <= ppe.Rating_Max);
      return risk.RiskCategory;
    }
    #endregion

    #region DCCalculation
    public decimal? CalcIncidentEnergyDC(CalculationInput calculationInput, decimal workingDistance)
    {
      if (!calculationInput.VoltageOfBattery.HasValue) return null;
      if (!calculationInput.MaximumShortCircuitAvailable.HasValue) return null;
      if (!calculationInput.InCabinet.HasValue) return null;

      var cabinet = (calculationInput.InCabinet.Value) ? 3m : 1m;
      decimal iEV = (cabinet * 0.01m * calculationInput.VoltageOfBattery.Value * calculationInput.MaximumShortCircuitAvailable.Value) / (decimal)Math.Pow((double)(workingDistance * 2.54m), (double)2m);

      return iEV;
    }

    public decimal? CalcWorkingDistanceDC(decimal incidentEnergy, CalculationInput calculationInput)
    {
      if (!calculationInput.VoltageOfBattery.HasValue) return null;
      if (!calculationInput.MaximumShortCircuitAvailable.HasValue) return null;
      if (!calculationInput.InCabinet.HasValue) return null;

      var cabinet = (calculationInput.InCabinet.Value) ? 3m : 1m;
      decimal wD = (decimal)Math.Sqrt((double)(0.01m * calculationInput.VoltageOfBattery.Value * (calculationInput.MaximumShortCircuitAvailable.Value / (incidentEnergy / cabinet)))) / 2.54m;

      return wD;
    }

    public decimal? CalcArcFlashBoundaryDC(CalculationInput calculationInput)
    {
      if (!calculationInput.VoltageOfBattery.HasValue) return null;
      if (!calculationInput.MaximumShortCircuitAvailable.HasValue) return null;
      if (!calculationInput.InCabinet.HasValue) return null;

      var cabinet = (calculationInput.InCabinet.Value) ? 3m : 1m;
      decimal aFB = ((decimal)(Math.Sqrt((double)(0.01m * calculationInput.MaximumShortCircuitAvailable.Value * 0.5m * calculationInput.VoltageOfBattery.Value * 2m / (1.2m / cabinet))))) / 2.54m;
      return aFB;
    }

    public decimal? CalcIncidentEnergyL50DC(CalculationInput calculationInput)
    {
      if (!calculationInput.MaximumShortCircuitAvailable.HasValue) return null;
      if (!calculationInput.InCabinet.HasValue) return null;

      var cabinet = (calculationInput.InCabinet.Value) ? 3m : 1m;
      decimal iE50 = cabinet * 0.01m * 48 * (calculationInput.MaximumShortCircuitAvailable.Value / 2m) * (2m / (45m * 45m));
      return iE50;
    }

    public string RiskCategoryDC(CalculationOutput calculationOutput)
    {
      var risk = _dataService.GetProtectiveEquipments().Find(ppe => ppe.Rating_Min <= calculationOutput.IncidentEnergy && ppe.Rating_Max >= calculationOutput.IncidentEnergy);
      return risk.RiskCategory;
    }

    public int GetInsulatingGloveClass()
    {
      if (ArcFlashIn.IsAlternatingCurrent.Value)
      {
        // AC

        // Class 0 – Nominal Voltage <= 1,000 V
        // Class 2 – Nominal Voltage <= 17,000 V
        // Class 4 – Nominal Voltage <= 36,000 V

        if (ArcFlashIn.NominalVoltage <= 1000)
          return 0;
        if (ArcFlashIn.NominalVoltage <= 17000)
          return 2;
        return 4;
      }
      // DC

      // Class 0 – Nominal Voltage <= 1,500 V
      // Class 2 – Nominal Voltage <= 25,500 V
      // Class 4 – Nominal Voltage <= 54,000 V
      if (ArcFlashIn.VoltageOfBattery <= 1500)
        return 0;
      if (ArcFlashIn.VoltageOfBattery <= 25500)
        return 2;
      return 4;
    }
    #endregion

    #region ArcDuration
    public decimal? CalculateArcDuration(CalculationInput calculationInput, decimal? estimatedArcFaultCurrent)
    {
      if (!calculationInput.SensorRating.HasValue || calculationInput.SensorRating ==0) return null;
      if (!estimatedArcFaultCurrent.HasValue) return null;
      if (!calculationInput.ArcDuration.LongTimePickup.HasValue) return null;
      if (calculationInput.ArcDuration.LongTimeDelay?.Value == null) return null;
      if (calculationInput.ArcDuration.ShortTimePickup?.Value == null) return null;
      if (calculationInput.ArcDuration.ShortTimeDelay?.Value == null) return null;
      if (calculationInput.ArcDuration.Instantaneous?.Value == null) return null;
      if (calculationInput.ArcDuration.ShortTimePickup.Value == "") return null;
      if (calculationInput.ArcDuration.ShortTimeDelay.Value == "") return null;
      if (calculationInput.ArcDuration.Instantaneous.Value == "") return null;

      // Maximum allowable duration
      decimal duration = 2;
      decimal estimatedArcFaultCurrentInAmps = estimatedArcFaultCurrent.Value * 1000;

      // Calculate the long time portion of the curve
      decimal long_time = calculationInput.ArcDuration.LongTimePickup.Value * calculationInput.SensorRating.Value;
      decimal long_time_duration = calculationInput.ArcDuration.LongTimeDelay.Value.Value * (decimal)(35 / ((Math.Pow((double)(estimatedArcFaultCurrentInAmps / long_time), 2)) - 1));

      duration = Math.Min(duration, long_time_duration);

      // Calculate the short time portion of the curve
      string shortTimePickup = calculationInput.ArcDuration.ShortTimePickup.Value;
      if (shortTimePickup != "N/A")
      {
        decimal stpu_d = Convert.ToDecimal(shortTimePickup);

        decimal short_time = stpu_d * long_time;

        if (short_time < estimatedArcFaultCurrentInAmps)
        {
          decimal short_time_duration = 2;

          string shortTimeDelay = calculationInput.ArcDuration.ShortTimeDelay.Value;
          if (shortTimeDelay.Contains("off"))
          {
            switch (shortTimeDelay.Replace(" off", ""))
            {
              case "0":
                short_time_duration = 0.08m;
                break;
              case "0.1":
                short_time_duration = 0.14m;
                break;
              case "0.2":
                short_time_duration = 0.2m;
                break;
              case "0.3":
                short_time_duration = 0.32m;
                break;
              default:
                short_time_duration = 0.5m;
                break;
            }
          }
          else if (shortTimeDelay.Contains("on"))
          {
            decimal std_d = Convert.ToDecimal(shortTimeDelay.Replace(" on", ""));

            short_time_duration = std_d * (decimal)(99 / ((Math.Pow((double)(estimatedArcFaultCurrentInAmps / short_time), 2)) - 1));
          }

          duration = Math.Min(duration, short_time_duration);
        }
      }

      // Calculate the instantaneous portion
      string instantaneos = calculationInput.ArcDuration.Instantaneous.Value;
      if (instantaneos != "off" && instantaneos != "N/A")
      {
        decimal inst_d = Convert.ToDecimal(instantaneos);

        decimal instantaneous = inst_d * calculationInput.SensorRating.Value;

        if (instantaneous < estimatedArcFaultCurrentInAmps)
        {
          duration = Math.Min(duration, 0.06m);
        }
      }

      return duration;
    }

    public decimal? CalcWorkingDistanceAC(decimal incidentEnergy, decimal? estimatedArcFaultCurrent)
    {
      if (!ArcFlashIn.ArcDurationValue.HasValue) return null;
      if (!estimatedArcFaultCurrent.HasValue) return null;

      decimal WD = 0m;
      if (ArcFlashIn.NominalVoltage > 15000)
      {
        WD = (decimal)Math.Pow((double)(2.142m * (decimal)Math.Pow(10, 6) * ArcFlashIn.NominalVoltage.Value * ArcFlashOut.BoltedFaultCurrent.Value * ArcFlashIn.ArcDurationValue.Value) / (double)(incidentEnergy * 4.184m * 1000m), 0.5) / 25.4m;
      }
      else
      {
        decimal XAC = CalcXAC(estimatedArcFaultCurrent);
        decimal voltageVal = (ArcFlashIn.NominalVoltage > 1000) ? 1m : 1.5m;
        WD = (decimal)Math.Pow((double)(voltageVal * XAC * ArcFlashIn.ArcDurationValue.Value * (decimal)Math.Pow(610, (double)Equipment.Distance_X_Fact)) / (double)(incidentEnergy * 0.2m), ((double)1m / (double)Equipment.Distance_X_Fact)) / 25.4m;
      }

      return WD;
    }
    #endregion

    public ICalculationOutput Calculate(CalculationInput calculationInput)
    {
      //set it
      ArcFlashIn = calculationInput;
      ArcFlashOut.InsulatingGloveClass = GetInsulatingGloveClass();

      //Standard Values
      if (ArcFlashIn.IsAlternatingCurrent.Value)
      {
        //get equipment
        Equipment = GetEquipment();

        //Set working distance
        if (!ArcFlashIn.HasCustomWorkingDistance)
          ArcFlashIn.WorkingDistance = GetWorkingDistance(Equipment.WorkingDistanceId);

        //Calculated Values
        ArcFlashOut.EstimatedArcFaultCurrent = CalculateArcFaultCurrentAC(calculationInput);
        ArcFlashOut.IE18 = CalcIncidentEnergyAC(18m, ArcFlashOut.EstimatedArcFaultCurrent).Value;
        ArcFlashOut.IE24 = CalcIncidentEnergyAC(24m, ArcFlashOut.EstimatedArcFaultCurrent).Value;
        ArcFlashOut.IE36 = CalcIncidentEnergyAC(36m, ArcFlashOut.EstimatedArcFaultCurrent).Value;
        ArcFlashOut.IncidentEnergy = CalcIncidentEnergyAC(ArcFlashIn.WorkingDistance.Value, ArcFlashOut.EstimatedArcFaultCurrent).Value;
        ArcFlashOut.FPB = CalcFlashHazardBoundaryAC(ArcFlashOut.EstimatedArcFaultCurrent);
        ArcFlashOut.HazardCat = RiskCategoryAC(ArcFlashOut);
        ArcFlashOut.MinimumWorkingDistanceOnePointTwoCal = CalcWorkingDistanceAC(1.2m, ArcFlashOut.EstimatedArcFaultCurrent).Value;
        ArcFlashOut.MinimumWorkingDistance8Cal = CalcWorkingDistanceAC(8, ArcFlashOut.EstimatedArcFaultCurrent).Value;
        ArcFlashOut.MinimumWorkingDistance40Cal = CalcWorkingDistanceAC(40, ArcFlashOut.EstimatedArcFaultCurrent).Value;

        //multiple of sensor rating
        ArcFlashOut.MultipleOfSensorRating = CalculateMultipleSensorRatingAC(calculationInput, ArcFlashOut.EstimatedArcFaultCurrent);
      }
      else
      {
        //From Input model
        ArcFlashOut.MASC = ArcFlashIn.MaximumShortCircuitAvailable.Value;

        //Set working distance
        if (!ArcFlashIn.HasCustomWorkingDistance)
          ArcFlashIn.WorkingDistance = STANDARD_WORKING_DISTANCE_DC_18_INCHES;

        //Calculated Values
        var calcIncidentEnergyDc = CalcIncidentEnergyDC(calculationInput, ArcFlashIn.WorkingDistance.Value);
        if (calcIncidentEnergyDc != null)
          ArcFlashOut.IncidentEnergy = calcIncidentEnergyDc.Value;

        var calcWorkingDistanceDc = CalcWorkingDistanceDC(ArcFlashIn.WorkingDistance.Value, calculationInput);
        if (calcWorkingDistanceDc != null)
          ArcFlashOut.MinWorkinDistance = calcWorkingDistanceDc.Value;

        var calcArcFlashBoundaryDc = CalcArcFlashBoundaryDC(calculationInput);
        if (calcArcFlashBoundaryDc != null)
          ArcFlashOut.ArcFlashBoundary = calcArcFlashBoundaryDc.Value;

        var calcIncidentEnergyL50Dc = CalcIncidentEnergyL50DC(calculationInput);
        if (calcIncidentEnergyL50Dc != null)
          ArcFlashOut.IE50v = calcIncidentEnergyL50Dc.Value;

        ArcFlashOut.HazardCat = RiskCategoryDC(ArcFlashOut);
        ArcFlashOut.MinimumWorkingDistanceOnePointTwoCal = CalcWorkingDistanceDC(1.2m, ArcFlashIn).Value;
        ArcFlashOut.MinimumWorkingDistance8Cal = CalcWorkingDistanceDC(8m, ArcFlashIn).Value;
        ArcFlashOut.MinimumWorkingDistance40Cal = CalcWorkingDistanceDC(40, ArcFlashIn).Value;

      }

      return ArcFlashOut;
    }
  }
}
