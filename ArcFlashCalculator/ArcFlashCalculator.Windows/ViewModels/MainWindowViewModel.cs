using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.WPF.Commands;
using ArcFlashCalculator.WPF.Controls;
using ArcFlashCalculator.WPF.Views;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ArcFlashCalculator.Core;
using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.WPF.Views.Modals;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ArcFlashCalculator.WPF.ViewModels
{
  public class MainWindowViewModel : BindableBase
  {
    private int _screenNumber = 1;
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private readonly IUnityContainer _container;
    private ISaveAndLoad _saveAndLoad;
    private IPdfCreatorService _pdfCreatorService;


    public DelegateCommand GoBackCommand { get; set; }
    public RelayCommand StartOverCommand { get; set; }
    public RelayCommand NextCommand { get; set; }
    public RelayCommand SettingsCommand { get; set; }
    public RelayCommand CalculationHistoryCommand { get; set; }
    public RelayCommand ExportCommand { get; set; }
    
    public MainWindowViewModel(ISaveAndLoad saveAndLoad, IPdfCreatorService pdfCreatorService, IEventAggregator eventAggregator, IRegionManager regionManager, IUnityContainer container)
    {
      //pdf generation
      _saveAndLoad = saveAndLoad;
      _pdfCreatorService = pdfCreatorService;

      //setup modal regions
      _regionManager = regionManager;
      _regionManager.RegisterViewWithRegion("ArcDurationCalculationRegion", typeof(ArcDurationCalculationView));
      _regionManager.RegisterViewWithRegion("FindSafeWorkingDistanceRegion", typeof(FindSafeWorkingDistanceView));
      _regionManager.RegisterViewWithRegion("FindEquipmentTripCurvesRegion", typeof(FindEquipmentTripCurvesView));
      _regionManager.RegisterViewWithRegion("StandardWorkingDistanceRegion", typeof(StandardWorkingDistancesView));
      _regionManager.RegisterViewWithRegion("SettingsRegion", typeof(SettingsView));
      
      //setup main regions
      _regionManager.RegisterViewWithRegion("ContentRegion", typeof(FormView));
      _regionManager.RegisterViewWithRegion("ContentRegion", typeof(ConfirmationView));
      _regionManager.RegisterViewWithRegion("ContentRegion", typeof(ResultsView));

      //container
      _container = container;

      //setup events
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(UpdateCalculationInput);
      _eventAggregator.GetEvent<ArcDurationUpdated>().Subscribe(UpdateArcDuration);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(HandleCalculationOutputUpdated);
      _eventAggregator.GetEvent<OpenModalEvent>().Subscribe(HandleOpenModalEvent);
      _eventAggregator.GetEvent<CloseModalEvent>().Subscribe(HandleCloseModalEvent);
      _eventAggregator.GetEvent<UseForNewCalculationClicked>().Subscribe(HandleUseForNewCalculationClicked);

      //setup commands
      GoBackCommand = new DelegateCommand(GoBack);
      StartOverCommand = new RelayCommand(ClickStartOver, CanClickStartOver);
      NextCommand = new RelayCommand(ClickNext, CanClickNext);
      
      SettingsCommand = new RelayCommand(ShowSettings);
      CalculationHistoryCommand = new RelayCommand(ShowCalculationHistory);
      ExportCommand = new RelayCommand(DoExport);
      
      //defaults
      HasErrors = true;
    }

    private void HandleOpenModalEvent(string viewName)
    {
      IsModalVisible = true;
      if (viewName == Constants.SETTINGS_VIEW) ShowSettingsModal = true;
      if (viewName == Constants.ARCDURATION_CALCULATION_VIEW) ShowCalculateArcDurationModal = true;
      if (viewName == Constants.FIND_SAFE_WORKING_DISTANCE_VIEW) ShowFindSafeWorkingDistanceModal = true;
      if (viewName == Constants.FIND_EQUIPMENT_TRIP_CURVES_VIEW) ShowFindTripCurvesModal = true;
      if (viewName == Constants.STANDARD_WORKING_DISTANCE_VIEW) ShowStandardWorkingDistanceModal = true;
      if (viewName == Constants.CALCULATION_HISTORY_VIEW) ShowCalculationHistoryModal = true;
    }

    private void HandleCloseModalEvent(string viewName)
    {
      IsModalVisible = false;
      if (viewName == Constants.SETTINGS_VIEW) ShowSettingsModal = false;
      if (viewName == Constants.ARCDURATION_CALCULATION_VIEW) ShowCalculateArcDurationModal = false;
      if (viewName == Constants.FIND_SAFE_WORKING_DISTANCE_VIEW) ShowFindSafeWorkingDistanceModal = false;
      if (viewName == Constants.FIND_EQUIPMENT_TRIP_CURVES_VIEW) ShowFindTripCurvesModal = false;
      if (viewName == Constants.STANDARD_WORKING_DISTANCE_VIEW) ShowStandardWorkingDistanceModal = false;
      if (viewName == Constants.CALCULATION_HISTORY_VIEW) ShowCalculationHistoryModal = false;
    }


    private void HandleUseForNewCalculationClicked(bool clicked)
    {
      StartOver(true);
    }


    private void ResetUseForNewCalculation()
    {
      CalculationInput.IsOpenAir = null;
      CalculationInput.IsSolidGround = null;
      CalculationInput.NominalVoltage = null;
      CalculationInput.SourceFaultCurrent = CalculationOutput.BoltedFaultCurrent * 1000;
      CalculationInput.HasTransformer = null;
      CalculationInput.HasCable = null;
      CalculationInput.SensorRating = null;
      CalculationInput.ArcDuration = _container.Resolve<IArcDuration>() as ArcDuration;
      CalculationInput.ArcDurationValue = null;
      CalculationInput.EquipmentType = null;
      CalculationOutput = _container.Resolve<ICalculationOutput>() as CalculationOutput;
    }

    private void HandleCalculationOutputUpdated(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
    }

    private void DoExport(object obj)
    {
      //Process.Start(filename);
      MemoryStream memoryStream = _pdfCreatorService.CreatePdfMemoryStream(CalculationInput, CalculationOutput);
      _saveAndLoad.SaveAndCreatePdfAsync("ArcFlashEstimateReport.pdf", memoryStream);
      // ...and start a viewer.
      Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\ArcFlashEstimateReport.pdf");
    }

    private void ShowSettings(object obj)
    {
      ShowSettingsModal = true;
      IsModalVisible = true;
    }

    private void ShowCalculationHistory(object obj)
    {
      ShowCalculationHistoryModal = true;
      IsModalVisible = true;
    }

    private void ClickNext(object obj)
    {
      string uri = Constants.FORM_VIEW;
      CalculationInput.HasCustomWorkingDistance = false;
      ScreenVisibility(_screenNumber);
      if (_screenNumber == 1)
      {
        uri = Constants.CONFIRMATION_VIEW;
        NextButtonText = AppResources.Calculate;
        ScreenVisibility(_screenNumber);
      }
      else
      {
        uri = Constants.RESULTS_VIEW;
        ShowResetButton = false;
        ShowNextButton = false;
        ScreenVisibility(_screenNumber);
        if (_screenNumber == 2)
        {
          ShowArhiveButton = true;
          ShowExportButton = true;
        }
      }

      _screenNumber++;
      Navigate(uri);
      ShowResetButton = false;
      ShowBackButton = true;
      IsStartOverButtonEnabled = true;
      ScreenVisibility(_screenNumber);
      _eventAggregator.GetEvent<CalculateButtonClicked>().Publish(true);
    }

    private void ClickStartOver(object obj)
    {
      StartOver(false);
    }

    private void StartOver(bool useForNewCalculation)
    {
      _screenNumber = 1;
      ShowResetButton = true;
      ShowBackButton = false;
      ShowArhiveButton = false;
      ShowExportButton = false;
      NextButtonText = AppResources.Next;
      if (useForNewCalculation)
      {
        ResetUseForNewCalculation();
      }
      else
      {
        //reset inputs
        ResetInputs();
      }
      UpdateCalculationInput(CalculationInput);
      Navigate(Constants.FORM_VIEW);
      HasErrors = true;
      ShowNextButton = true;
    }

    private void ResetInputs()
    {
      CalculationInput.Personnel = "";
      CalculationInput.Location = "";
      CalculationInput.Action = "";
      CalculationInput.CalculationDate = DateTime.Now;
      CalculationInput.IsAlternatingCurrent = null;
      CalculationInput.EquipmentType = null;
      CalculationInput.EquipmentTypeId = null;
      CalculationInput.IsSolidGround = null;
      CalculationInput.NominalVoltage = null;
      CalculationInput.SourceFaultCurrent = null;
      CalculationInput.IsOpenAir = null;
      CalculationInput.HasTransformer = null;
      CalculationInput.PrimaryVoltage = null;
      CalculationInput.XfmrImpedance = null;
      CalculationInput.XfmrKVA = null;
      CalculationInput.HasCable = null;
      CalculationInput.ConductorLength = null;
      CalculationInput.ConductorPerPhase = null;
      CalculationInput.ConductorSize = null;
      CalculationInput.SensorRating = null;

      CalculationInput.ArcDurationValue = null;
      CalculationInput.ArcDuration.Manufacturer = null;
      CalculationInput.ArcDuration.BreakerStyle = null;
      CalculationInput.ArcDuration.TripUnitType = null;
      CalculationInput.ArcDuration.LongTimePickup = null;
      CalculationInput.ArcDuration.LongTimeDelay = null;
      CalculationInput.ArcDuration.ShortTimePickup = null;
      CalculationInput.ArcDuration.ShortTimeDelay = null;
      CalculationInput.ArcDuration.Instantaneous = null;

      CalculationInput.MaximumShortCircuitAvailable = null;
      CalculationInput.InCabinet = null;
      CalculationInput.VoltageOfBattery = null;
    }

    public bool HasMultipleCalculations { get; set; }

    private bool CanClickStartOver(object obj)
    {
      return CalculationInput != null && CalculationInput.IsAlternatingCurrent.HasValue;
    }

    private void GoBack()
    {
      var uri = _screenNumber == 3 ? Constants.CONFIRMATION_VIEW : Constants.FORM_VIEW;
      Navigate(uri);
      _screenNumber--;
      ShowBackButton = _screenNumber > 1;
      ShowResetButton = _screenNumber == 1;
      NextButtonText = _screenNumber == 2 ? AppResources.Calculate : AppResources.Next;
      ShowNextButton = true;
      ShowArhiveButton = false;
      ShowExportButton = false;
      ScreenVisibility(_screenNumber);
    }

    private void UpdateCalculationInput(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      HasRangeErrors = calculationInput.ErrorCount > 0;
      HasErrors = HasRangeErrors || calculationInput.HasErrors;
    }

    private void UpdateArcDuration(ArcDuration arcDuration)
    {
      CalculationInput.ArcDuration = arcDuration;
    }

    private void Navigate(string uri)
    {
      _regionManager.RequestNavigate("ContentRegion", uri);
    }

    public bool CanClickNext(object p)
    {
      CommandManager.InvalidateRequerySuggested();
      return !HasErrors;
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

    private bool _showBackButton;
    public bool ShowBackButton
    {
      get { return _showBackButton; }
      set { SetProperty(ref _showBackButton, value); }
    }

		private bool _showArhiveButton;

    public bool ShowArhiveButton
    {
      get { return _showArhiveButton; }
      set { SetProperty(ref _showArhiveButton, value); }
    }

		private bool _showExportButton;

    public bool ShowExportButton
    {
      get { return _showExportButton; }
      set { SetProperty(ref _showExportButton, value); }
    }

    private bool _showResetButton = true;
    public bool ShowResetButton
    {
      get { return _showResetButton; }
      set { SetProperty(ref _showResetButton, value); }
    }

    private bool _showNextButton = true;
    public bool ShowNextButton
    {
      get { return _showNextButton; }
      set { SetProperty(ref _showNextButton, value); }
    }

    private string _nextButtonText = AppResources.Next;
    public string NextButtonText
    {
      get { return _nextButtonText; }
      set { SetProperty(ref _nextButtonText, value); }
    }

    private bool _showCalculateButton;
    public bool ShowCalculateButton
    {
      get { return _showCalculateButton; }
      set { SetProperty(ref _showCalculateButton, value); }
    }

		private bool _showFindTripCurvesModal;

    public bool ShowFindTripCurvesModal
    {
      get { return _showFindTripCurvesModal; }
      set { SetProperty(ref _showFindTripCurvesModal, value); }
    }

		private bool _showSetCalculationNameModal;

    public bool ShowSetCalculationNameModal
    {
      get { return _showSetCalculationNameModal; }
      set { SetProperty(ref _showSetCalculationNameModal, value); }
    }

    private bool _showStandardWorkingDistanceModal;

    public bool ShowStandardWorkingDistanceModal
    {
      get { return _showStandardWorkingDistanceModal; }
      set { SetProperty(ref _showStandardWorkingDistanceModal, value); }
    }

		private bool _showSettingsModal;

    public bool ShowSettingsModal
    {
      get { return _showSettingsModal; }
      set { SetProperty(ref _showSettingsModal, value); }
    }

    private bool _showCalculationHistoryModal;

    public bool ShowCalculationHistoryModal
    {
      get { return _showCalculationHistoryModal; }
      set { SetProperty(ref _showCalculationHistoryModal, value); }
    }

    private bool _isModalVisible;

    public bool IsModalVisible
    {
      get { return _isModalVisible; }
      set { SetProperty(ref _isModalVisible, value); }
    }

    private int _errorCount;
    public int ErrorCount
    {
      get { return _errorCount; }
      set { SetProperty(ref _errorCount, value); }
    }

    private bool _hasNoErrors;
    public bool HasNoErrors
    {
      get { return _hasNoErrors; }
      set { SetProperty(ref _hasNoErrors, value); }
    }

    private bool _hasErrors;
    public bool HasErrors
    {
      get { return _hasErrors; }
      set
      {
        SetProperty(ref _hasErrors, value);
        HasNoErrors = !HasErrors;
      }
    }

    private bool _hasRangeErrors;
    public bool HasRangeErrors
    {
      get { return _hasRangeErrors; }
      set { SetProperty(ref _hasRangeErrors, value); }
    }

    private bool _isStartOverButtonEnabled;
    public bool IsStartOverButtonEnabled
    {
      get { return _isStartOverButtonEnabled; }
      set { SetProperty(ref _isStartOverButtonEnabled, value); }
    }

    private bool _showCalculateArcDurationModal;

    public bool ShowCalculateArcDurationModal
    {
      get { return _showCalculateArcDurationModal; }
      set { SetProperty(ref _showCalculateArcDurationModal, value); }
    }

    private bool _showFindSafeWorkingDistanceModal;

    public bool ShowFindSafeWorkingDistanceModal
    {
      get { return _showFindSafeWorkingDistanceModal; }
      set { SetProperty(ref _showFindSafeWorkingDistanceModal, value); }
    }

    private bool _isFormScreenVisible = false;
    public bool IsFormScreenVisible
    {
      get
      {
        return _screenNumber == 1;
      }
      set { SetProperty(ref _isFormScreenVisible, value); }
    }

    private bool _isConfirmationScreenVisible;

    public bool IsConfirmationScreenVisible
    {
      get { return _isConfirmationScreenVisible; }
      set { SetProperty(ref _isConfirmationScreenVisible, value); }
    }

    private bool _isResultsScreenVisible;

    public bool IsResultsScreenVisible
    {
      get { return _isResultsScreenVisible; }
      set { SetProperty(ref _isResultsScreenVisible, value); }
    }

    private decimal? _incidentEnergy;

    public decimal? IncidentEnergy
    {
      get { return _incidentEnergy; }
      set { SetProperty(ref _incidentEnergy, value); }
    }

    void ScreenVisibility(int screenNo)
    {
      if (screenNo == 1)
      {
        IsFormScreenVisible = true;
        IsConfirmationScreenVisible = false;
        IsResultsScreenVisible = false;
      }
      else if (screenNo == 2)
      {
        IsFormScreenVisible = false;
        IsConfirmationScreenVisible = true;
        IsResultsScreenVisible = false;
      }
      else
      {
        IsFormScreenVisible = false;
        IsConfirmationScreenVisible = false;
        IsResultsScreenVisible = true;
      }
    }
  }
}
