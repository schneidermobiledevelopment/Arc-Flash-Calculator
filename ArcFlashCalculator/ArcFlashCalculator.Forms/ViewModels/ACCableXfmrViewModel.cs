﻿using ArcFlashCalculator.Core.Events;
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


namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ACCableXfmrViewModel : BindableBase
  {
    private INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IUnityContainer _container;
    private readonly IValidator<CalculationInput> _validator;
    private readonly IEventAggregator _eventAggregator;

    public ACCableXfmrViewModel(INavigationService navigationService, IDataService dataService, ICalculationInput calculationInput, IValidator<CalculationInput> validator, IUnityContainer container, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _dataService = dataService;
      _calculationInput = calculationInput as CalculationInput;
      _validator = validator;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
    }
    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      ConductorPerPhaseErrorMessage = CalculationInput.GetStringError("ConductorPerPhase");
      ConductorLengthErrorMessage = CalculationInput.GetStringError("ConductorLenght");
      HasErrors = ConductorPerPhaseErrorMessage != null || ConductorLengthErrorMessage != null;
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
    public ObservableCollection<ConductorSize> ConductorSizes => new ObservableCollection<ConductorSize>(_dataService.GetConductorSizes());

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

    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver);

    private async void StartOver()
    {
      CalculationInput = _container.Resolve<ICalculationInput>() as CalculationInput;
      await _navigationService.NavigateAsync("app:///NavigationPage/CommonInput");
    }
    public void OnNavigatedFrom(NavigationParameters parameters)
    {

    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      if (parameters.ContainsKey("calculationInput"))
        HandleCalculationInputUpdated((CalculationInput)parameters["calculationInput"]);
    }
  }
}