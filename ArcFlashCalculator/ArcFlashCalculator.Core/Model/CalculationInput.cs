using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using Prism.Events;

namespace ArcFlashCalculator.Core.Model
{
  public class CalculationInput : ValidatableBindableBase, INotifyDataErrorInfo, ICalculationInput
  {
    private Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
    private readonly IValidator<CalculationInput> _validator;
    private readonly IEventAggregator _eventAggregator;

    public CalculationInput(IEventAggregator eventAggregator, IValidator<CalculationInput> validator, IArcDuration arcDuration)
    {
      _eventAggregator = eventAggregator;
      _validator = validator;
      _arcDuration = arcDuration as ArcDuration;
    }
    public CalculationInput() { }
    public int Id { get; set; }

    public decimal ArcDurationMin => 0.01m;
    public decimal ArcDurationMax => 2;

    public decimal NominalVoltageMin => 30;
    public decimal NominalVoltageLowVoltageMax => 1000;
    public decimal NominalVoltageMediumVoltageMin => 1001;
    public decimal NominalVoltageMax => 36000;

    public decimal SourceFaultCurrentMin => 1;
    public decimal SourceFaultCurrentMax => 99999;

    public decimal PrimaryVoltageMin => 208;
    public decimal PrimaryVoltageMax => 138000;

    public decimal XfmrImpedanceMin => 1;
    public decimal XfmrImpedanceMax => 15;

    public decimal MaximumAvailableShortCircuitMin => 1000;
    public decimal MaximumAvailableShortCircuitMax => 50000;

    public decimal VoltageOfBatteryMin => 12;
    public decimal VoltageOfBatteryMax => 600;

    public decimal SensorRatingMin => 15;
    public decimal SensorRatingMax => 6000;

    public decimal XfmrKVAMin => 0;

    string _personnel;
    public string Personnel
    {
      get { return _personnel; }
      set { DoSetterWork(ref _personnel, value); }
    }

    private string _location;
    public string Location
    {
      get { return _location; }
      set { DoSetterWork(ref _location, value); }
    }

    private string _action;
    public string Action
    {
      get { return _action; }
      set { DoSetterWork(ref _action, value); }
    }

    private DateTime _calculationDate = DateTime.Now;
    public DateTime CalculationDate
    {
      get { return _calculationDate; }
      set { DoSetterWork(ref _calculationDate, value); }
    }

    private bool? _isAlternatingCurrent = null;
    public bool? IsAlternatingCurrent
    {
      get { return _isAlternatingCurrent; }
      set
      {
        DoSetterWork(ref _isAlternatingCurrent, value);
        if (value != null && (bool)!value)
        {
          IsArcDurationCalculated = false;
        }
      }
    }

    private EquipmentType _equipmentType;
    public EquipmentType EquipmentType
    {
      get { return _equipmentType; }
      set
      {
        DoSetterWork(ref _equipmentType, value);
        if (_equipmentType!=null)
        {
          EquipmentTypeId = _equipmentType.Id;
        }
        else
        {
          EquipmentTypeId = null;
        }
      }
    }

    private int? _equipmentTypeId;
    public int? EquipmentTypeId
    {
      get { return _equipmentTypeId; }
      set
      {
        DoSetterWork(ref _equipmentTypeId, value);
        if (NominalVoltage.HasValue)
        {
          RaiseErrorsChanged("NominalVoltage");
        }
      }
    }

    private bool? _isSolidGround;
    public bool? IsSolidGround
    {
      get { return _isSolidGround; }
      set { DoSetterWork(ref _isSolidGround, value); }
    }

    private decimal? _arcDurationValue;
    public decimal? ArcDurationValue
    {
      get { return _arcDurationValue; }
      set
      {
        DoSetterWork(ref _arcDurationValue, value);
        RaiseErrorsChanged("ArcDurationValue");
      }
    }

    private bool _isArcDurationCalculated;
    public bool IsArcDurationCalculated
    {
      get { return _isArcDurationCalculated; }
      set { DoSetterWork(ref _isArcDurationCalculated, value); }
    }

    private ArcDuration _arcDuration;
    public ArcDuration ArcDuration
    {
      get { return _arcDuration; }
      set
      {
        DoSetterWork(ref _arcDuration, value);
      }
    }

    private int? _sensorRating;
    public int? SensorRating
    {
      get { return _sensorRating; }
      set
      {
        DoSetterWork(ref _sensorRating, value);
        RaiseErrorsChanged("SensorRating");
      }
    }

    private decimal? _workingDistance;
    public decimal? WorkingDistance
    {
      get { return _workingDistance; }
      set { DoSetterWork(ref _workingDistance, value); }
    }

    private bool _hasCustomWorkingDistance;
    public bool HasCustomWorkingDistance
    {
      get { return _hasCustomWorkingDistance; }
      set { DoSetterWork(ref _hasCustomWorkingDistance, value); }
    }

    private decimal? _nominalVoltage;
    public decimal? NominalVoltage
    {
      get { return _nominalVoltage; }
      set
      {
        DoSetterWork(ref _nominalVoltage, value);
      }
    }

    private decimal? _sourceFaultCurrent;
    public decimal? SourceFaultCurrent
    {
      get { return _sourceFaultCurrent; }
      set
      {
        DoSetterWork(ref _sourceFaultCurrent, value);
      }
    }

    private bool? _isOpenAir;
    public bool? IsOpenAir
    {
      get { return _isOpenAir; }
      set { DoSetterWork(ref _isOpenAir, value); }
    }

    private bool? _hasCable;

    public bool? HasCable
    {
      get { return _hasCable; }
      set
      {
        DoSetterWork(ref _hasCable, value);
        if (!_hasCable.HasValue || !_hasCable.Value)
        {
          ConductorSize = null;
          ConductorPerPhase = null;
          ConductorLength = null;
        }
      }
    }
 
    private ConductorSize _conductorSize;

    public ConductorSize ConductorSize
    {
      get { return _conductorSize; }
      set
      {
        DoSetterWork(ref _conductorSize, value);
        if (_conductorSize != null)
        {
          ConductorSizeId = _conductorSize.Id;
        }
      }
    }

    public int ConductorSizeId { get; set; }

    private int? _conductorPerPhase;
    public int? ConductorPerPhase
    {
      get { return _conductorPerPhase; }
      set
      {
        DoSetterWork(ref _conductorPerPhase, value);
      }
    }

    /// <summary>/// Prism Property/// </summary>
		private int? _conductorLength;

    public int? ConductorLength
    {
      get { return _conductorLength; }
      set
      {
        DoSetterWork(ref _conductorLength, value);
      }
    }

    /// <summary>/// Prism Property/// </summary>
		private UnitOfMeasure _conductorLengthUnitOfMeasure;
    public UnitOfMeasure ConductorLengthUnitOfMeasure
    {
      get { return _conductorLengthUnitOfMeasure; }
      set { DoSetterWork(ref _conductorLengthUnitOfMeasure, value); }
    }

    private bool? _hasTransformer;
    public bool? HasTransformer
    {
      get { return _hasTransformer; }
      set
      {
        DoSetterWork(ref _hasTransformer, value);
        if (!_hasTransformer.HasValue || !_hasTransformer.Value)
        {
          PrimaryVoltage = null;
          XfmrImpedance = null;
          XfmrKVA = null;
        }
      }
    }

    private decimal? _primaryVoltage;
    public decimal? PrimaryVoltage
    {
      get { return _primaryVoltage; }
      set
      {
        DoSetterWork(ref _primaryVoltage, value);
      }
    }

    /// <summary>/// Prism Property/// </summary>
		private decimal? _xfmrImpedance;
    public decimal? XfmrImpedance
    {
      get { return _xfmrImpedance; }
      set
      {
        DoSetterWork(ref _xfmrImpedance, value);
      }
    }
    /// <summary>/// Prism Property/// </summary>
		private decimal? _xfmrKVA;
    public decimal? XfmrKVA
    {
      get { return _xfmrKVA; }
      set
      {
        DoSetterWork(ref _xfmrKVA, value);
      }
    }

    //DC
    private int? _maximumShortCircuitAvailableDecimal;
    public int? MaximumShortCircuitAvailable
    {
      get { return _maximumShortCircuitAvailableDecimal; }
      set { DoSetterWork(ref _maximumShortCircuitAvailableDecimal, value); }
    }

    private bool? _inCabinet;
    public bool? InCabinet
    {
      get { return _inCabinet; }
      set { DoSetterWork(ref _inCabinet, value); }
    }

    private int? _voltageOfBattery;
    public int? VoltageOfBattery
    {
      get { return _voltageOfBattery; }
      set { DoSetterWork(ref _voltageOfBattery, value); }
    }

    public new event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

    private void RaiseErrorsChanged(string propertyName)
    {
      if (ErrorsChanged != null)
        ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
    }
    public override void ValidateAllProperties()
    {
      _propertyErrors = this._validator.Validate(this) as Dictionary<string, List<string>>;
    }

    public override void UpdateErrors()
    {
      _propertyErrors = this._validator.Validate(this) as Dictionary<string, List<string>>;
    }

    public override void PublishObject()
    {
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(this);
    }

    public IEnumerable GetErrors(string propertyName)
    {
      lock (_propertyErrors)
      {
        if (_propertyErrors.ContainsKey(propertyName))
        {
          return _propertyErrors[propertyName];
        }
        return null;
      }
    }

    public string GetStringError(string propertyName)
    {
      lock (_propertyErrors)
      {
        if (_propertyErrors.ContainsKey(propertyName) && _propertyErrors[propertyName].First() != "Required Field")
        {
          return _propertyErrors[propertyName].First();
        }
        return null;
      }
    }

    public bool HasErrors
    {
      get { return PropertyErrors(); }
    }
    public int ErrorCount
    {
      get { return _propertyErrors.Count(o => o.Value.First() != "Required Field"); }
    }

    private bool PropertyErrors()
    {
      return _propertyErrors.Keys.Any(key => _propertyErrors[key] != null);
    }
  }
}
