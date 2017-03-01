using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ACInputOneViewModel : BindableBase, INavigationAware
  {

    #region Fields
    private readonly IDataService _dataService;
    private readonly IUnityContainer _container;
    private readonly IValidator<CalculationInput> _validator;
    private CalculationInput _calculationInput;
    private INavigationService _navigationService;
    private readonly IEventAggregator _eventAggregator;
    private string _sourceFaultCurrentErrorMessage;
    private string _nominalVoltageErrorMessage;
    private bool _isClicked = false;
    private bool _isEnabled;
    private bool _hasErrors;
    #endregion


    #region Contructors
    public ACInputOneViewModel(INavigationService navigationService, IDataService dataService, ICalculationInput calculationInput, IValidator<CalculationInput> validator, IUnityContainer container, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _dataService = dataService;
      _calculationInput = calculationInput as CalculationInput;
      _validator = validator;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
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

    private string _field;
    public string Field
    {
      get { return _field; }
      set { SetProperty(ref _field, value); }
    }
    public IList<EquipmentType> EquipmentTypes => new List<EquipmentType>(_dataService.GetEquipmentTypes());
    
    public string NominalVoltageErrorMessage
    {
      get { return _nominalVoltageErrorMessage; }
      set { SetProperty(ref _nominalVoltageErrorMessage, value); }
    }
    
    public string SourceFaultCurrentErrorMessage
    {
      get { return _sourceFaultCurrentErrorMessage; }
      set { SetProperty(ref _sourceFaultCurrentErrorMessage, value); }
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
    public DelegateCommand NextCommand => new DelegateCommand(Next,CanNext);
    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver,CanStartOver);
    public DelegateCommand HelpCommand => new DelegateCommand(Help, CanHelp);
    
    #endregion

    #region Methods

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      NominalVoltageErrorMessage = CalculationInput.GetStringError("NominalVoltage");
      SourceFaultCurrentErrorMessage = CalculationInput.GetStringError("SourceFaultCurrent");
      HasErrors = NominalVoltageErrorMessage !=null || SourceFaultCurrentErrorMessage != null;
      IsEnabled = (CalculationInput.EquipmentType!=null && CalculationInput.IsSolidGround.HasValue && CalculationInput.NominalVoltage.HasValue && CalculationInput.SourceFaultCurrent.HasValue && !HasErrors);
      
    }

    private bool CanNext()
    {
      return (!_isClicked && IsEnabled);
    }

    private void Reset()
    {
      CalculationInput.EquipmentType = default(EquipmentType);
      CalculationInput.IsSolidGround = default(bool?);
      CalculationInput.NominalVoltage = default(decimal?);
      CalculationInput.SourceFaultCurrent = default(decimal?);
    }

    private async void Next()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("calculationInput", _calculationInput);
      await _navigationService.NavigateAsync("ACInputTwo", p);
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
      p.Add("help", "ACInputOne");
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
      if (parameters.ContainsKey("Field"))
        Field = (string)parameters["Field"];
    }
    #endregion

  }
}
