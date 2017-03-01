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
using System.Collections.ObjectModel;
using System.Linq;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ACHasCableViewModel : BindableBase, INavigationAware
  {
    private INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IUnityContainer _container;
    private readonly IValidator<CalculationInput> _validator;
    private readonly IEventAggregator _eventAggregator;

    #region Constructor
    public ACHasCableViewModel(INavigationService navigationService, IDataService dataService, ICalculationInput calculationInput, IValidator<CalculationInput> validator, IUnityContainer container, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _dataService = dataService;
      _calculationInput = calculationInput as CalculationInput;
      _validator = validator;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated, ThreadOption.UIThread, false, obj => obj.HasCable == true);
    }
    #endregion

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      ConductorPerPhaseErrorMessage = CalculationInput.GetStringError("ConductorPerPhase");
      ConductorLengthErrorMessage = CalculationInput.GetStringError("ConductorLenght");
      HasErrors = ConductorPerPhaseErrorMessage != null || ConductorLengthErrorMessage !=null;
      IsEnabled = (CalculationInput.ConductorSize != null && CalculationInput.ConductorPerPhase.HasValue && CalculationInput.ConductorLength.HasValue && !HasErrors);

    }

    private string _conductorPerPhaseErrorMessage;
    public string ConductorPerPhaseErrorMessage
    {
      get { return _conductorPerPhaseErrorMessage; }
      set { SetProperty(ref _conductorPerPhaseErrorMessage, value); }
    }
    private string _conductorLengthErrorMessage;
    public string ConductorLengthErrorMessage
    {
      get { return _conductorLengthErrorMessage; }
      set { SetProperty(ref _conductorLengthErrorMessage, value); }
    }

    private bool _isClicked = false;
    private bool _isEnabled;
    public bool IsEnabled
    {
      get { return _isEnabled; }
      set { SetProperty(ref _isEnabled, value); }
    }
    private string _field;
    public string Field
    {
      get { return _field; }
      set { SetProperty(ref _field, value); }
    }

    private bool _hasErrors;
    public bool HasErrors
    {
      get { return _hasErrors; }
      set { SetProperty(ref _hasErrors, value); }
    }
    private CalculationInput _calculationInput;
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      {
        SetProperty(ref _calculationInput, value);
      }
    }
    public IList<ConductorSize> ConductorSizes => new List<ConductorSize>(_dataService.GetConductorSizes());

    public DelegateCommand ResetCommand => new DelegateCommand(Reset);

    private void Reset()
    {
      CalculationInput.ConductorSize = default(ConductorSize);
      if (CalculationInput.ConductorPerPhase.HasValue)
        CalculationInput.ConductorPerPhase = default(int?);
      if (CalculationInput.ConductorLength.HasValue)
        CalculationInput.ConductorLength = default(int?);
      
    }
    private bool CanNext()
    {
      return (!_isClicked && IsEnabled);
    }
    public DelegateCommand NextCommand => new DelegateCommand(Next, CanNext);

    private async void Next()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("calculationInput", _calculationInput);
      if (CalculationInput.HasTransformer.Value)
        await _navigationService.NavigateAsync("ACHasXfmr", p);
      else
        await _navigationService.NavigateAsync("ACSensor", p);
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

    public DelegateCommand HelpCommand => new DelegateCommand(Help, CanHelp);
    private bool CanHelp()
    {
      return !_isClicked;
    }
    private async void Help()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("help", "ACHasCable");
      await _navigationService.NavigateAsync("NavPage/TechnicalHelp", p, true);
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