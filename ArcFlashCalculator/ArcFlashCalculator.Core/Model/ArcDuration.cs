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
  public class ArcDuration : ValidatableBindableBase, INotifyDataErrorInfo, IArcDuration
  {
    private Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
    private readonly IValidator<ArcDuration> _validator;
    private IEventAggregator _eventAggregator;

    public ArcDuration(IEventAggregator eventAggregator, IValidator<ArcDuration> validator)
    {
      _eventAggregator = eventAggregator;
      _validator = validator;
    }

    public decimal LongTimePickupMin => .4m;
    public decimal LongTimePickupMax => 1;
    public decimal SensorRatingMin => 15;
    public decimal SensorRatingMax => 6000;

    private Manufacturer _manufacturer;
    public Manufacturer Manufacturer
    {
      get { return _manufacturer; }
      set { DoSetterWork(ref _manufacturer, value); }
    }

    private BreakerStyle _breakerStyle;
    public BreakerStyle BreakerStyle
    {
      get { return _breakerStyle; }
      set { DoSetterWork(ref _breakerStyle, value); }
    }

    private TripUnitType _tripUnitType;
    public TripUnitType TripUnitType
    {
      get { return _tripUnitType; }
      set
      {
        DoSetterWork(ref _tripUnitType, value);
      }
    }

    private decimal? _longTimePickup;
    public decimal? LongTimePickup
    {
      get { return _longTimePickup; }
      set
      {
        DoSetterWork(ref _longTimePickup, value);
      }
    }


    private LongTimeDelay _longTimeDelay;
    public LongTimeDelay LongTimeDelay
    {
      get { return _longTimeDelay; }
      set
      {
        DoSetterWork(ref _longTimeDelay, value);
      }
    }

    private ShortTimePickup _shortTimePickup;
    public ShortTimePickup ShortTimePickup
    {
      get { return _shortTimePickup; }
      set
      {
        DoSetterWork(ref _shortTimePickup, value);
      }
    }

    private ShortTimeDelay _shortTimeDelay;
    public ShortTimeDelay ShortTimeDelay
    {
      get { return _shortTimeDelay; }
      set
      {
        DoSetterWork(ref _shortTimeDelay, value);
      }
    }

    private Instantaneous _instantaneous;
    public Instantaneous Instantaneous
    {
      get { return _instantaneous; }
      set
      {
        DoSetterWork(ref _instantaneous, value);
      }
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
      _eventAggregator.GetEvent<ArcDurationUpdated>().Publish(this);
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

    public new event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

    private void RaiseErrorsChanged(string propertyName)
    {
      if (ErrorsChanged != null)
        ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
    }
  }
}
