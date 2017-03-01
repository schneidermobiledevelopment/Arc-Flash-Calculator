using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Prism.Events;
using Prism.Navigation;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class CalculateArcDurationOneViewModel : BindableBase, INavigationAware
  {

    #region Fields
    private readonly IDataService _dataService;
    private readonly IUnityContainer _container;
    private readonly INavigationService _navigationService;
    private readonly IEventAggregator _eventAggregator;
    private bool _isClicked = false;
    private bool _hasErrors;
    private bool _isCancel;
    #endregion

    public CalculateArcDurationOneViewModel(INavigationService navigationService, IDataService dataService, IEventAggregator eventAggregator, IUnityContainer container, IArcDuration arcDuration, ICalculationOutput calculationOutput, ICalculationInput calculationInput)
    {
      _navigationService = navigationService;
      _dataService = dataService;
      _arcDuration = arcDuration as ArcDuration;
      _calculationInput = calculationInput as CalculationInput;
      _calculationOutput = calculationOutput as CalculationOutput;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<ArcDurationUpdated>().Subscribe(UpdateArcDuration);
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated, ThreadOption.UIThread, false,obj => obj.IsArcDurationCalculated==true);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(HandleCalculationOutputUpdated);
    }

    #region Properties
    public IList<Manufacturer> Manufacturers => new List<Manufacturer>(_dataService.GetManufacturers());
    private IList<BreakerStyle> _breakerStyles;
    public IList<BreakerStyle> BreakerStyles
    {
      get { return _breakerStyles; }
      set { SetProperty(ref _breakerStyles, value); }
    }

    private IList<TripUnitType> _tripUnitTypes;
    public IList<TripUnitType> TripUnitTypes
    {
      get { return _tripUnitTypes; }
      set { SetProperty(ref _tripUnitTypes, value); }
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
    private ArcDuration _arcDuration;
    public ArcDuration ArcDuration
    {
      get { return _arcDuration; }
      set { SetProperty(ref _arcDuration, value); }
    }

    private bool _breakerEnabled;
    public bool BreakerEnabled
    {
      get { return _breakerEnabled; }
      set { SetProperty(ref _breakerEnabled, value); }
    }

    private string _sensorRatingErrorMessage;
    public string SensorRatingErrorMessage
    {
      get { return _sensorRatingErrorMessage; }
      set { SetProperty(ref _sensorRatingErrorMessage, value); }
    }


    private bool _tripUnitEnabled;
    public bool TripUnitEnabled
    {
      get { return _tripUnitEnabled; }
      set { SetProperty(ref _tripUnitEnabled, value); }
    }

    private bool _isEnabled;
    public bool IsEnabled
    {
      get { return _isEnabled; }
      set { SetProperty(ref _isEnabled, value); }
    }

    #endregion

    #region  Methods
    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      if (calculationInput.ArcDuration.Manufacturer != null)
      {
        BreakerEnabled = true;
        BreakerStyles = null;
        BreakerStyles = new List<BreakerStyle>(_dataService.GetBreakerStyles().Where(o => o.ManufacturerId == calculationInput.ArcDuration.Manufacturer.Id).ToList());
        if (calculationInput.ArcDuration.BreakerStyle != null)
        {
          TripUnitEnabled = true;
          TripUnitTypes = _dataService.GetTripUnitTypes().Where(o => o.BreakerStyleId == calculationInput.ArcDuration.BreakerStyle.Id).ToList();
        }
      }
      CalculationInput = calculationInput;
      //if (CalculationInput.ArcDuration.Manufacturer != null && BreakerEnabled == false)
      //{
      //  BreakerEnabled = true;
      //  BreakerStyles = _dataService.GetBreakerStyles().Where(o => o.ManufacturerId == CalculationInput.ArcDuration.Manufacturer.Id).ToList();
      //}
      //if (CalculationInput.ArcDuration.BreakerStyle != null && TripUnitEnabled == false)
      //{
      //  TripUnitEnabled = true;
      //  TripUnitTypes = _dataService.GetTripUnitTypes().Where(o => o.BreakerStyleId == CalculationInput.ArcDuration.BreakerStyle.Id).ToList();
      //}
      SensorRatingErrorMessage = CalculationInput.GetStringError("SensorRating");
      IsEnabled = (CalculationInput.SensorRating.HasValue && SensorRatingErrorMessage==null && CalculationInput.ArcDuration.Manufacturer != null && CalculationInput.ArcDuration != null && CalculationInput.ArcDuration.TripUnitType != null);

    }

    private void HandleCalculationOutputUpdated(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
    }

    private void UpdateArcDuration(ArcDuration arcDuration)
    {
      ArcDuration = arcDuration;
      if (CalculationInput.ArcDuration.Manufacturer != null && BreakerEnabled == false)
      {
        BreakerEnabled = true;
        BreakerStyles = new List<BreakerStyle>(_dataService.GetBreakerStyles().Where(o => o.ManufacturerId == CalculationInput.ArcDuration.Manufacturer.Id).ToList());
      }
      if (CalculationInput.ArcDuration.BreakerStyle != null && TripUnitEnabled == false)
      {
        TripUnitEnabled = true;
        TripUnitTypes = _dataService.GetTripUnitTypes().Where(o => o.BreakerStyleId == CalculationInput.ArcDuration.BreakerStyle.Id).ToList();
      }
      SensorRatingErrorMessage = CalculationInput.GetStringError("SensorRating");
      IsEnabled = (CalculationInput.SensorRating.HasValue && SensorRatingErrorMessage == null && CalculationInput.ArcDuration.Manufacturer != null && CalculationInput.ArcDuration != null && CalculationInput.ArcDuration.TripUnitType != null);
    }
    private bool CanClick()
    {
      return !_isClicked;
    }
    private bool CanNext()
    {
      return (CanClick() && IsEnabled);
    }
    #endregion

    #region Commands

    public  DelegateCommand CancelCommand => new DelegateCommand(Cancel,CanClick);

    private void Cancel()
    {
      CalculationInput.IsArcDurationCalculated = false;
      CalculationInput.ArcDuration = _container.Resolve<IArcDuration>() as ArcDuration;
      CalculationInput.ArcDurationValue = default(decimal?);
      _navigationService.GoBackAsync(null,true);
    }
    public  DelegateCommand NextCommand => new DelegateCommand(Next,CanNext);

    

    private async void Next()
    {var p = new NavigationParameters();
      // p.Add("ArcDuration",ArcDuration);
      p.Add("CalculationOutput", CalculationOutput);
      p.Add("CalculationInput",CalculationInput);
      await _navigationService.NavigateAsync("CalculateArcDurationTwo",p);
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
    #endregion

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Publish(CalculationOutput);
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
      _eventAggregator.GetEvent<ArcDurationUpdated>().Publish(ArcDuration);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
     //if (parameters.ContainsKey("CalculationOutput"))
     //   CalculationOutput= ((CalculationOutput)parameters["CalculationOutput"]);
      //if (parameters.ContainsKey("CalculationInput"))
      //{
      //  if (((CalculationInput) parameters["CalculationInput"]).ArcDuration.Manufacturer != null)
      //  {
      //    BreakerEnabled = true;
      //    BreakerStyles = _dataService.GetBreakerStyles().Where(o => o.ManufacturerId == ((CalculationInput)parameters["CalculationInput"]).ArcDuration.Manufacturer.Id).ToList();
      //    if (((CalculationInput) parameters["CalculationInput"]).ArcDuration.BreakerStyle != null)
      //    {
      //      TripUnitEnabled = true;
      //      TripUnitTypes = _dataService.GetTripUnitTypes().Where(o => o.BreakerStyleId == ((CalculationInput)parameters["CalculationInput"]).ArcDuration.BreakerStyle.Id).ToList();
      //    }
      //  }
      //  CalculationInput = ((CalculationInput) parameters["CalculationInput"]);
       
      //}
      //  if (parameters.ContainsKey("Field"))
      //Field = (string)parameters["Field"];
    }
  }
}
