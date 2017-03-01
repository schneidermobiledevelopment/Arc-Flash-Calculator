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
  public class ACInputTwoViewModel : BindableBase, INavigationAware
  {
    #region Fields
    private INavigationService _navigationService;
    private readonly IUnityContainer _container;
    private readonly IValidator<CalculationInput> _validator;
    private CalculationInput _calculationInput;
    private readonly IEventAggregator _eventAggregator;
    private bool _isClicked = false;
    private bool _isEnabled;
    private bool _hasErrors;
    #endregion

    #region Properties

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
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      {
        SetProperty(ref _calculationInput, value);
      }
    }
    #endregion

    #region Constructor
    public ACInputTwoViewModel(INavigationService navigationService, ICalculationInput calculationInput, IValidator<CalculationInput> validator, IUnityContainer container, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _calculationInput = calculationInput as CalculationInput;
      _validator = validator;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
    }
    #endregion

    #region Commands

    public DelegateCommand ResetCommand => new DelegateCommand(Reset);
    public DelegateCommand NextCommand => new DelegateCommand(Next, CanNext);
    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver,CanStartOver);
    public DelegateCommand HelpCommand => new DelegateCommand(Help,CanHelp);
    #endregion


    #region Methods

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      //HasErrors = CalculationInput.ErrorCount != 0;
      IsEnabled = (CalculationInput.IsOpenAir.HasValue && CalculationInput.HasCable.HasValue && CalculationInput.HasTransformer.HasValue);
    }

    private bool CanNext()
    {
      return (!_isClicked && IsEnabled);
    }

    private void Reset()
    {
      CalculationInput.IsOpenAir = default(bool?);
      CalculationInput.HasCable = default(bool?);
      CalculationInput.HasTransformer = default(bool?);
    }

    private async void Next()
    {
        _isClicked = true;
        var p = new NavigationParameters();
        p.Add("calculationInput", _calculationInput);
      if (CalculationInput.HasCable.Value && CalculationInput.HasTransformer.Value)
      {
        await _navigationService.NavigateAsync("ACCableAndXfmr", p);
      }
      else if (CalculationInput.HasCable.Value)
      {
        CalculationInput.XfmrImpedance = default(decimal?);
        CalculationInput.PrimaryVoltage = default(decimal?);
        CalculationInput.XfmrKVA = default(decimal?);
        await _navigationService.NavigateAsync("ACHasCable", p);
      }
      else if (CalculationInput.HasTransformer.Value)
      {
        CalculationInput.ConductorSize = default(ConductorSize);
        CalculationInput.ConductorPerPhase = default(int?);
        CalculationInput.ConductorLength = default(int?);
        await _navigationService.NavigateAsync("ACHasXfmr", p);
      }
      else
      {
        CalculationInput.ConductorSize = default(ConductorSize);
        CalculationInput.ConductorPerPhase = default(int?);
        CalculationInput.ConductorLength = default(int?);
        CalculationInput.XfmrImpedance = default(decimal?);
        CalculationInput.PrimaryVoltage = default(decimal?);
        CalculationInput.XfmrKVA = default(decimal?);
        await _navigationService.NavigateAsync("ACSensor", p);
      }
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
    private bool CanHelp()
    {
      return !_isClicked;
    }

    private async void Help()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("help", "ACInputTwo");
      await _navigationService.NavigateAsync("NavPage/TechnicalHelp", p, true);
      _isClicked = false;
    }
    #endregion

    #region NavigationAware
    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      //if (parameters.ContainsKey("calculationInput"))
      //  HandleCalculationInputUpdated((CalculationInput)parameters["calculationInput"]);
    }
    #endregion
  }
}