using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ArcFlashCalculator.Core.Interfaces;

namespace ArcFlashCalculator.Core.Model
{
  public class CalculationOutput : BaseModel, ICalculationOutput
  {
    //by Calculation
    private decimal? _estimatedArcFaultCurrent;

    public decimal? EstimatedArcFaultCurrent
    {
      get { return _estimatedArcFaultCurrent; }
      set
      {
        SetProperty(ref _estimatedArcFaultCurrent, value);
      }
    }
    
    private decimal? _multipleOfSensorRating;

    public decimal? MultipleOfSensorRating
    {
      get { return _multipleOfSensorRating; }
      set { SetProperty(ref _multipleOfSensorRating, value); }
    }

    private decimal? _boltedFaultCurrent;
    public decimal? BoltedFaultCurrent
    {
      get { return _boltedFaultCurrent; }
      set { SetProperty(ref _boltedFaultCurrent, value); }
    }

    private decimal _IncidentEnergyecimal;
    public decimal IncidentEnergy
    {
      get { return _IncidentEnergyecimal; }
      set { SetProperty(ref _IncidentEnergyecimal, value); }
    }
		private string _hazardCat;
    public string HazardCat
    {
      get { return _hazardCat; }
      set { SetProperty(ref _hazardCat, value); }
    }

		private decimal _fpb;
    public decimal FPB
    {
      get { return _fpb; }
      set { SetProperty(ref _fpb, value); }
    }

		private decimal _ie18;
    public decimal IE18// Incident Energy 18"
    {
      get { return _ie18; }
      set { SetProperty(ref _ie18, value); }
    }

    private decimal _ie24;
    public decimal IE24// Incident Energy 24"
    {
      get { return _ie24; }
      set { SetProperty(ref _ie24, value); }
    }

    private decimal _ie36;
    public decimal IE36// Incident Energy 36"
    {
      get { return _ie36; }
      set { SetProperty(ref _ie36, value); }
    }

    private decimal _minimumWorkingDistance8Cal;
    public decimal MinimumWorkingDistance8Cal 
    {
      get { return _minimumWorkingDistance8Cal; }
      set { SetProperty(ref _minimumWorkingDistance8Cal, value); }
    }

    private decimal _MinimumWorkingDistanceOnePointTwoCal;
    public decimal MinimumWorkingDistanceOnePointTwoCal 
    {
      get { return _MinimumWorkingDistanceOnePointTwoCal; }
      set { SetProperty(ref _MinimumWorkingDistanceOnePointTwoCal, value); }
    }

    private decimal _minimumWorkingDistance40Cal;
    public decimal MinimumWorkingDistance40Cal 
    {
      get { return _minimumWorkingDistance40Cal; }
      set { SetProperty(ref _minimumWorkingDistance40Cal, value); }
    }

    //for DC
		private decimal _minWorkingDistance;

    public decimal MinWorkinDistance
    {
      get { return _minWorkingDistance; }
      set { SetProperty(ref _minWorkingDistance, value); }
    }

    //HazardCat is common
		private decimal _arcFlashBoundary;
    public decimal ArcFlashBoundary
    {
      get { return _arcFlashBoundary; }
      set { SetProperty(ref _arcFlashBoundary, value); }
    }

    //Incident Energy at less than 50 volts
    private decimal _ie50v;
    public decimal IE50v
    {
      get { return _ie50v; }
      set { SetProperty(ref _ie50v, value); }
    }

		private decimal _masc;
    public decimal MASC// Maximum Available Short Circuit (amps)
    {
      get { return _masc; }
      set { SetProperty(ref _masc, value); }
    }

    private decimal _insulatingGloveClass;
    public decimal InsulatingGloveClass// Maximum Available Short Circuit (amps)
    {
      get { return _insulatingGloveClass; }
      set { SetProperty(ref _insulatingGloveClass, value); }
    }
  }
}
