using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ACSensorViewModel : BindableBase, INavigationAware
  {
    #region Fields
    private INavigationService _navigationService;
    private readonly IUnityContainer _container;
    private readonly IValidator<CalculationInput> _validator;
    private readonly ICalculatorService _calculatorService;
    private CalculationInput _calculationInput;
    private CalculationOutput _calculationOutput;
    private readonly IEventAggregator _eventAggregator;
    private string _sensorRatingErrorMessage;
    private string _arcDurationValueErrorMessage;
    private bool _isClicked = false;
    private bool _isEnabled;
    private bool _hasErrors;
    private bool _inSensor;
    private bool _sensorChanged;
    #endregion

    #region Constructor
    public ACSensorViewModel(INavigationService navigationService,ICalculatorService calculatorService, ICalculationInput calculationInput, ICalculationOutput calculationOutput, IValidator<CalculationInput> validator, IUnityContainer container, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _calculationInput = calculationInput as CalculationInput;
      _calculationOutput = calculationOutput as CalculationOutput;
      _validator = validator;
      _container = container;
      _calculatorService = calculatorService;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(HandleCalculationOutputUpdated);

    }

    #endregion

    #region Properties
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      {
        SetProperty(ref _calculationInput, value);
      }
    }
    private decimal? _estimatedArcFaultCurrent;
    public decimal? EstimatedArcFaultCurrent 
    {
      get { return _estimatedArcFaultCurrent; }
      set { SetProperty(ref _estimatedArcFaultCurrent, value); }
    }
    private string _field;
    public string Field
    {
      get { return _field; }
      set { SetProperty(ref _field, value); }
    }
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set { SetProperty(ref _calculationOutput, value); }
    }
    public string SensorRatingErrorMessage
    {
      get { return _sensorRatingErrorMessage; }
      set { SetProperty(ref _sensorRatingErrorMessage, value); }
    }

    public string ArcDurationValueErrorMessage
    {
      get { return _arcDurationValueErrorMessage; }
      set { SetProperty(ref _arcDurationValueErrorMessage, value); }
    }
    public bool IsEnabled
    {
      get { return _isEnabled; }
      set { SetProperty(ref _isEnabled, value); }
    }
    public bool HasErrors
    {
      get { return _hasErrors; }
      set { SetProperty(ref _hasErrors, value); }
    }
    #endregion

    #region Commands
    public DelegateCommand ResetCommand => new DelegateCommand(Reset);
    public DelegateCommand NextCommand => new DelegateCommand(Next, CanNext);
    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver, CanStartOver);
    public DelegateCommand HelpCommand => new DelegateCommand(Help, CanClick);
    public DelegateCommand TripCurvesCommand => new DelegateCommand(TripCurves, CanClick);
    public DelegateCommand CalculateArcDurationCommand => new DelegateCommand(CalculateArcDuration, CanClick);
    public DelegateCommand ArcDurationTextChangedCommand => new DelegateCommand(ArcDurationTextChanged);
    public DelegateCommand SensorRatingTextChangedCommand => new DelegateCommand(SensorRatingTextChanged);

    #endregion

    #region Methods

    private void HandleCalculationOutputUpdated(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
    }

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      SensorRatingErrorMessage = CalculationInput.GetStringError("SensorRating");
      ArcDurationValueErrorMessage = CalculationInput.GetStringError("ArcDurationValue");
      CalculationOutput.EstimatedArcFaultCurrent = _calculatorService.CalculateArcFaultCurrentAC(CalculationInput);
      EstimatedArcFaultCurrent = CalculationOutput.EstimatedArcFaultCurrent*1000;
      CalculationOutput.MultipleOfSensorRating = _calculatorService.CalculateMultipleSensorRatingAC(CalculationInput, CalculationOutput.EstimatedArcFaultCurrent);
      HasErrors = SensorRatingErrorMessage != null || ArcDurationValueErrorMessage != null;
      IsEnabled = ( CalculationInput.ArcDurationValue.HasValue && !HasErrors);
    }

    private void SensorRatingTextChanged()
    {
      _sensorChanged = true;
    }

    private void ArcDurationTextChanged()
    {
      if (CalculationInput.IsArcDurationCalculated && _inSensor && !_sensorChanged)
      {
            CalculationInput.IsArcDurationCalculated = false;
            CalculationInput.ArcDuration = _container.Resolve<IArcDuration>() as ArcDuration;
            _sensorChanged = false;
      }
      if (_sensorChanged)
      {
        _sensorChanged = false;
      }
    }
    private bool CanNext()
    {
      return (!_isClicked && IsEnabled);
    }

    private void Reset()
    {
      CalculationInput.SensorRating = default(int?);
      CalculationInput.ArcDurationValue = null;
    }

    private async void Next()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("calculationInput", _calculationInput);
      p.Add("CalculationOutput",CalculationOutput);
      await _navigationService.NavigateAsync("ACCircuitDiagram", p);
      _isClicked = false;
    }

    private bool CanStartOver()
    {
      return !_isClicked;
    }

    private async void StartOver()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new StartOverPopup());
      _isClicked = false;
    }

    public bool CanClick()
    {
      return !_isClicked;
    }

    private async void Help()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("help", "ACSensor");
      await _navigationService.NavigateAsync("NavPage/TechnicalHelp", p, true);
      _isClicked = false;
    }

    private async void TripCurves()
    {
      _isClicked = true;
      await _navigationService.NavigateAsync("NavPage/TripCurves", null, true);
      _isClicked = false;
    }

    private async void CalculateArcDuration()
    {
      _inSensor = false;
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("CalculationOutput", CalculationOutput);
      p.Add("CalculationInput",CalculationInput);
      CalculationInput.IsArcDurationCalculated = true;
      await _navigationService.NavigateAsync("NavPage/CalculateArcDurationOne", p, true);
      _isClicked = false;
    }
    #endregion

    #region NavigationAware

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Publish(CalculationOutput);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      _inSensor = true;
      //if (parameters.ContainsKey("calculationInput")) 
      //  HandleCalculationInputUpdated((CalculationInput)parameters["calculationInput"]);
        
        if (parameters.ContainsKey("Field"))
          Field = (string)parameters["Field"];
      
    }
    #endregion
  }
}