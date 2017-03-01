using System;
using System.Linq;
using ArcFlashCalculator.Core;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.WPF.Commands;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ArcFlashCalculator.WPF.ViewModels.Modals
{
  public class ArcDurationCalculationViewModel : BindableBase
  {
    private readonly IDataService _dataService;
    private readonly IEventAggregator _eventAggregator;
    private readonly ICalculatorService _calculatorService;
    private readonly IUnityContainer _unityContainer;

    public ArcDurationCalculationViewModel(IEventAggregator eventAggregator, IDataService dataService, ICalculatorService calculatorService, IUnityContainer unityContainer)
    {
      //events
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(UpdateCalculationInput);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(UpdateCalculationOutput);
      _eventAggregator.GetEvent<ArcDurationUpdated>().Subscribe(UpdateArcDuration);

      //data service
      _dataService = dataService;

      //calc service
      _calculatorService = calculatorService;

      //container
      _unityContainer = unityContainer;

      Manufacturers = new ItemListViewModel<Manufacturer>();
      BreakerStyles = new ItemListViewModel<BreakerStyle>();
      TripUnitTypes = new ItemListViewModel<TripUnitType>();
      LongTimeDelays = new ItemListViewModel<LongTimeDelay>();
      ShortTimePickups = new ItemListViewModel<ShortTimePickup>();
      ShortTimeDelays = new ItemListViewModel<ShortTimeDelay>();
      Instantaneous = new ItemListViewModel<Instantaneous>();

      Manufacturers.PropertyChanged += (s, e) => Update(e.PropertyName, Manufacturers, BreakerStyles, LoadBreakerTypes);
      BreakerStyles.PropertyChanged += (s, e) => Update(e.PropertyName, BreakerStyles, TripUnitTypes, LoadTripUnitTypes);
      TripUnitTypes.PropertyChanged += (s, e) => Update(e.PropertyName, TripUnitTypes, LongTimeDelays, LoadLongTimeDelays);
      TripUnitTypes.PropertyChanged += (s, e) => Update(e.PropertyName, TripUnitTypes, ShortTimePickups, LoadShortTimePickups);
      TripUnitTypes.PropertyChanged += (s, e) => Update(e.PropertyName, TripUnitTypes, ShortTimeDelays, LoadShortTimeDelays);
      TripUnitTypes.PropertyChanged += (s, e) => Update(e.PropertyName, TripUnitTypes, Instantaneous, LoadInstantaneous);

      //load manufacturers
      Manufacturers.SetItems(_dataService.GetManufacturers());

      //commands
      CanceModalCommand = new DelegateCommand(CancelModal);
      NextCommand = new RelayCommand(ClickNext, CanClickNext);

    }

    private bool CanClickNext(object obj)
    {
      return !HasErrors;
    }

    private void ClickNext(object obj)
    {
      CalculationInput.IsArcDurationCalculated = true;
      _eventAggregator.GetEvent<CloseModalEvent>().Publish(Constants.ARCDURATION_CALCULATION_VIEW);
    }

    private void UpdateArcDuration(ArcDuration arcDuration)
    {
      if(arcDuration.Manufacturer == null)
        Manufacturers.SelectedItem = arcDuration.Manufacturer;
      if (arcDuration.BreakerStyle == null)
        BreakerStyles.SelectedItem = arcDuration.BreakerStyle;
      if (arcDuration.TripUnitType == null)
        TripUnitTypes.SelectedItem = arcDuration.TripUnitType;

      HasRangeErrors = CalculationInput.ArcDuration.ErrorCount > 0;
      HasErrors = HasRangeErrors || CalculationInput.ArcDuration.HasErrors;
      HasCalculation = CalculationInput.ArcDurationValue.HasValue;
      if (CalculationOutput.EstimatedArcFaultCurrent != null)
        CalculationInput.ArcDurationValue = _calculatorService.CalculateArcDuration(CalculationInput, CalculationOutput.EstimatedArcFaultCurrent.Value);

    }

    private void UpdateCalculationOutput(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
    }

    private void UpdateCalculationInput(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
    }

    private void CancelModal()
    {
      CalculationInput.IsArcDurationCalculated = false;
      CalculationInput.ArcDuration = _unityContainer.Resolve<IArcDuration>() as ArcDuration;
      CalculationInput.ArcDurationValue = null;
      _eventAggregator.GetEvent<CloseModalEvent>().Publish(Constants.ARCDURATION_CALCULATION_VIEW);
    }

    public DelegateCommand CanceModalCommand { get; set; }
    public RelayCommand NextCommand { get; set; }
    public ItemListViewModel<Manufacturer> Manufacturers { get; set; }
    public ItemListViewModel<BreakerStyle> BreakerStyles { get; set; }
    public ItemListViewModel<TripUnitType> TripUnitTypes { get; set; }
    public ItemListViewModel<LongTimeDelay> LongTimeDelays { get; set; }
    public ItemListViewModel<ShortTimePickup> ShortTimePickups { get; set; }
    public ItemListViewModel<ShortTimeDelay> ShortTimeDelays { get; set; }
    public ItemListViewModel<Instantaneous> Instantaneous { get; set; }

    private CalculationInput _calculationInput;

    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set { SetProperty(ref _calculationInput, value); }
    }

    /// <summary>/// Prism Property/// </summary>
		private CalculationOutput _calculationOutput;
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set { SetProperty(ref _calculationOutput, value); }
    }

    private bool _hasErrors = true;
    public bool HasErrors
    {
      get { return _hasErrors; }
      set { SetProperty(ref _hasErrors, value); }
    }

    private bool _hasRangeErrors;
    public bool HasRangeErrors
    {
      get { return _hasRangeErrors; }
      set { SetProperty(ref _hasRangeErrors, value); }
    }

    private void LoadBreakerTypes()
    {
      CalculationInput.ArcDuration.Manufacturer = Manufacturers.SelectedItem;
      BreakerStyles.SetItems(_dataService.GetBreakerStyles().Where(o => o.ManufacturerId == CalculationInput.ArcDuration.Manufacturer.Id));
    }

    private void LoadTripUnitTypes()
    {
      CalculationInput.ArcDuration.BreakerStyle = BreakerStyles.SelectedItem;
      TripUnitTypes.SetItems(_dataService.GetTripUnitTypes().Where(o => o.BreakerStyleId == CalculationInput.ArcDuration.BreakerStyle.Id));
    }

    private void LoadLongTimeDelays()
    {
      CalculationInput.ArcDuration.TripUnitType = TripUnitTypes.SelectedItem;
      LongTimeDelays.SetItems(_dataService.GetLongTimeDelays().Where(o => o.TripUnitTypeId == CalculationInput.ArcDuration.TripUnitType.Id));
      TripUnitTypeSelected = true; //For showing rest of dropdowns
    }

    private void LoadShortTimePickups()
    {
      ShortTimePickups.SetItems(_dataService.GetShortTimePickups().Where(o => o.TripUnitTypeId == TripUnitTypes.SelectedItem.Id));
    }

    private void LoadShortTimeDelays()
    {
      ShortTimeDelays.SetItems(_dataService.GetShortTimeDelays().Where(o => o.TripUnitTypeId == TripUnitTypes.SelectedItem.Id));
    }
    private void LoadInstantaneous()
    {
      Instantaneous.SetItems(_dataService.GetInstantaneous().Where(o => o.TripUnitTypeId == TripUnitTypes.SelectedItem.Id));
    }

    private void Update<T0, T1>(string propertyName, ItemListViewModel<T0> parent, ItemListViewModel<T1> child, Action loadAction)
        where T0 : class
        where T1 : class
    {
      if (propertyName == "SelectedItem")
      {
        if (parent.SelectedItem == null)
        {
          child.SetItems(Enumerable.Empty<T1>());
        }
        else
        {
          loadAction();
        }
      }
    }

    /// <summary>/// Prism Property/// </summary>
    private bool _tripUnitTypeSelected;

    public bool TripUnitTypeSelected
    {
      get { return _tripUnitTypeSelected; }
      set { SetProperty(ref _tripUnitTypeSelected, value); }
    }

    private bool _hasCalculation;

    public bool HasCalculation
    {
      get { return _hasCalculation; }
      set { SetProperty(ref _hasCalculation, value); }
    }
  }
}
