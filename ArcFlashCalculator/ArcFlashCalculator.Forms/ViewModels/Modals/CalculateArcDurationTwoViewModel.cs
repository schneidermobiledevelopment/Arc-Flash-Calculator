using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Navigation;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class CalculateArcDurationTwoViewModel : BindableBase, INavigationAware
  {
    #region  Fields

    private readonly IDataService _dataService;
    private readonly IUnityContainer _container;
    private readonly INavigationService _navigationService;
    private readonly IEventAggregator _eventAggregator;
    private readonly ICalculatorService _calculatorService;
    private bool _isClicked = false;
    private bool _inModal;
    private bool _isCancel;


    #endregion
    public CalculateArcDurationTwoViewModel(INavigationService navigationService, IDataService dataService, IEventAggregator eventAggregator, IUnityContainer container, IArcDuration arcDuration, ICalculatorService calculatorService, ICalculationInput calculationInput, ICalculationOutput calculationOutput)
    {
      _inModal = true;
      _navigationService = navigationService;
      _dataService = dataService;
      _arcDuration = arcDuration as ArcDuration;
      _calculationInput = calculationInput as CalculationInput;
      _calculationOutput = calculationOutput as CalculationOutput;
      _calculatorService = calculatorService;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<ArcDurationUpdated>().Subscribe(UpdateArcDuration);
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated, ThreadOption.UIThread, false, obj => obj.IsArcDurationCalculated == true);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(HandleCalculationOutputUpdated);
    }

    #region Properties

    private string _longTimePickupErrorMessage;
    public string LongTimePickupErrorMessage
    {
      get { return _longTimePickupErrorMessage; }
      set { SetProperty(ref _longTimePickupErrorMessage, value); }
    }

    private ArcDuration _arcDuration;
    public ArcDuration ArcDuration
    {
      get { return _arcDuration; }
      set { SetProperty(ref _arcDuration, value); }
    }
    private bool _isEnabled;
    public bool IsEnabled
    {
      get { return _isEnabled; }
      set { SetProperty(ref _isEnabled, value); }
    }
    private CalculationInput _calculationInput;
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set { SetProperty(ref _calculationInput, value); }
    }
    private CalculationOutput _calculationOutput;
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set { SetProperty(ref _calculationOutput, value); }
    }

    public IList<LongTimeDelay> LongTimeDelays => new List<LongTimeDelay>(_dataService.GetLongTimeDelays());
    public  IList<ShortTimePickup> ShortTimePickups => new List<ShortTimePickup>(_dataService.GetShortTimePickups());
    public IList<ShortTimeDelay> ShortTimeDelays => new List<ShortTimeDelay>(_dataService.GetShortTimeDelays());
    public IList<Instantaneous> Instantaneous => new List<Instantaneous>(_dataService.GetInstantaneous());
    #endregion

    #region Methods
    private void HandleCalculationOutputUpdated(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
    }
    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      LongTimePickupErrorMessage = CalculationInput.ArcDuration.GetStringError("LongTimePickup");
      IsEnabled = (CalculationInput.ArcDuration.LongTimePickup.HasValue && LongTimePickupErrorMessage == null && CalculationInput.ArcDuration.LongTimeDelay != null &&
                   CalculationInput.ArcDuration.ShortTimePickup != null && CalculationInput.ArcDuration.ShortTimeDelay != null &&
                   CalculationInput.ArcDuration.Instantaneous != null);
      if(!_inModal && calculationInput.IsArcDurationCalculated)
        CalculationInput.ArcDurationValue = _calculatorService.CalculateArcDuration(CalculationInput, CalculationOutput.EstimatedArcFaultCurrent.Value);

      if (IsEnabled && _inModal && CalculationInput.ArcDuration.TripUnitType != null && CalculationInput.ArcDuration.Manufacturer != null && CalculationInput.ArcDuration.BreakerStyle != null && CalculationInput.SensorRating.HasValue)
      {
        CalculationInput.ArcDurationValue = _calculatorService.CalculateArcDuration(CalculationInput, CalculationOutput.EstimatedArcFaultCurrent.Value);
        CalculationInput.IsArcDurationCalculated = true;
      }

    }
    private void UpdateArcDuration(ArcDuration arcDuration)
    {
      ArcDuration = arcDuration;
      LongTimePickupErrorMessage = CalculationInput.ArcDuration.GetStringError("LongTimePickup");
      IsEnabled = (CalculationInput.ArcDuration.LongTimePickup.HasValue && LongTimePickupErrorMessage == null && CalculationInput.ArcDuration.LongTimeDelay != null &&
                   CalculationInput.ArcDuration.ShortTimePickup != null && CalculationInput.ArcDuration.ShortTimeDelay != null &&
                   CalculationInput.ArcDuration.Instantaneous != null);
      if (IsEnabled && CalculationInput.ArcDuration.TripUnitType != null && CalculationInput.ArcDuration.Manufacturer != null && CalculationInput.ArcDuration.BreakerStyle != null && CalculationInput.SensorRating.HasValue)
      {
        CalculationInput.ArcDurationValue = _calculatorService.CalculateArcDuration(CalculationInput, CalculationOutput.EstimatedArcFaultCurrent.Value);
        CalculationInput.ArcDuration = arcDuration;
        CalculationInput.IsArcDurationCalculated = true;
      }
    }
    private bool CanClick()
    {
      return !_isClicked;
    }
    private bool CanCalculate()
    {
      return (CanClick() && IsEnabled);
    }
    #endregion

    #region Commands
    public DelegateCommand CancelCommand=> new DelegateCommand(Cancel,CanClick);
    
    private void Cancel()
    {
      CalculationInput.IsArcDurationCalculated = false;
      CalculationInput.ArcDuration = _container.Resolve<IArcDuration>() as ArcDuration;
      CalculationInput.ArcDurationValue = default(decimal?);
      _inModal = false;
      _navigationService.GoBackAsync(null, true);
    }
    public DelegateCommand HelpCommand => new DelegateCommand(Help, CanClick);
    private async void Help()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("help", "ArcDuration");
      await _navigationService.NavigateAsync("NavPage/TechnicalHelp", p, true);
      _isClicked = false;
    }
    public DelegateCommand CalculateCommand => new DelegateCommand(Calculate, CanCalculate);

    private async void Calculate()
    {
      _inModal = false;
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("CalculationInput", CalculationInput);
      await _navigationService.GoBackAsync(null, true);
      _isClicked = false;
    }

    #endregion

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Publish(CalculationOutput);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      //if (parameters.ContainsKey("CalculationOutput"))
      //  CalculationOutput = ((CalculationOutput)parameters["CalculationOutput"]);
      //  if (parameters.ContainsKey("CalculationInput"))
      //  CalculationInput = ((CalculationInput) parameters["CalculationInput"]);
     
     
        //  if (parameters.ContainsKey("Field"))
        //Field = (string)parameters["Field"];
      }
  }
}
