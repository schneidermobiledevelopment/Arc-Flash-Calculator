using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class DCInputViewModel : BindableBase, INavigationAware
  {
    private INavigationService _navigationService;
    private readonly IUnityContainer _container;
    private readonly IValidator<CalculationInput> _validator;
    private CalculationInput _calculationInput;
    private readonly IEventAggregator _eventAggregator;
    private bool _isClicked = false;
    private bool _isEnabled;
    private bool _hasErrors;

    #region Constructor
    public DCInputViewModel(INavigationService navigationService, IUnityContainer container, ICalculationInput calculationInput, IValidator<CalculationInput> validator, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _calculationInput = calculationInput as CalculationInput;
      _validator = validator;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
    }

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      MaximumShortCircuitAvailableErrorMessage = CalculationInput.GetStringError("MaximumShortCircuitAvailable");
      VoltageOfBatteryErrorMessage = CalculationInput.GetStringError("VoltageOfBattery");
      HasErrors = MaximumShortCircuitAvailableErrorMessage != null || VoltageOfBatteryErrorMessage !=null;
      IsEnabled = (CalculationInput.MaximumShortCircuitAvailable.HasValue && CalculationInput.InCabinet.HasValue && CalculationInput.VoltageOfBattery.HasValue && !HasErrors);
    }

    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      {
        SetProperty(ref _calculationInput, value);
      }
    }

    public bool HasErrors
    {
      get {return _hasErrors; }
      set { SetProperty(ref _hasErrors, value); }
    }
    private string _maximumShortCircuitAvailableErrorMessage;
    public string MaximumShortCircuitAvailableErrorMessage
    {
      get { return _maximumShortCircuitAvailableErrorMessage; }
      set { SetProperty(ref _maximumShortCircuitAvailableErrorMessage, value); }
    }
    private string _voltageOfBatteryErrorMessage;
    public string VoltageOfBatteryErrorMessage
    {
      get { return _voltageOfBatteryErrorMessage; }
      set { SetProperty(ref _voltageOfBatteryErrorMessage, value); }
    }

    public bool IsEnabled
    {
      get
      {
        return _isEnabled;
      }
      set
      {
        SetProperty(ref _isEnabled, value);
      }
    }
    #endregion

    public DelegateCommand ResetCommand => new DelegateCommand(Reset);

    private void Reset()
    {
      CalculationInput.MaximumShortCircuitAvailable = default(int?);
      CalculationInput.InCabinet = default(bool?);
      CalculationInput.VoltageOfBattery = default(int?);
    }

    public DelegateCommand NextCommand => new DelegateCommand(Next, CanNavigate);

    private bool CanNavigate()
    {
      return (!_isClicked && IsEnabled);
    }

    public DelegateCommand HelpCommand => new DelegateCommand(Help, CanHelp);

    private bool CanHelp()
    {
      return !_isClicked;
    }
    private string _field;
    public string Field
    {
      get { return _field; }
      set { SetProperty(ref _field, value); }
    }
    private async void Help()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("help", "DCInput");
      await _navigationService.NavigateAsync("NavPage/TechnicalHelp", p, true);
      _isClicked = false;
    }
    private async void Next()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("calculationInput", _calculationInput);
      await _navigationService.NavigateAsync("DCCircuitDiagram",p);
      _isClicked = false;
    }

    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver, CanStartOver);

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

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      //if (parameters.ContainsKey("calculationInput"))
      //  HandleCalculationInputUpdated((CalculationInput)parameters["calculationInput"]);
      if (parameters.ContainsKey("Field"))
        Field = (string)parameters["Field"];
    }
  }
}
