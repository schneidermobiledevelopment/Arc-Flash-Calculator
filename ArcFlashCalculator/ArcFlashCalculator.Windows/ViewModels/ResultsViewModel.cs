using System;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.WPF.Controls;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using ArcFlashCalculator.Core;
using ArcFlashCalculator.Core.Locals;
using Prism.Commands;

namespace ArcFlashCalculator.WPF.ViewModels
{
  public class ResultsViewModel : BindableBase
  {
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private ICalculatorService _calculatorService;
    private IShowingAndHidingService _showingAndHidingService;

    public ResultsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ICalculatorService calculatorService, IShowingAndHidingService showingAndHidingService)
    {
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(InputUpdated, ThreadOption.UIThread, true);
      _eventAggregator.GetEvent<CalculateButtonClicked>().Subscribe(CalculateClick, ThreadOption.UIThread, true);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(HandleCalculationOutputUpdated);

      _regionManager = regionManager;
      _regionManager.RegisterViewWithRegion("CircuitDiagramResultsRegion", typeof(DiagramView));
      _calculatorService = calculatorService;

      _showingAndHidingService = showingAndHidingService;

      FindSafeWorkingDistanceCommand = new DelegateCommand(FindSafeWorkingDistance);
      StandardWorkingDistanceCommand = new DelegateCommand(StandardWorkingDistance);
      UseForNewCalculation = new DelegateCommand(HandleUseForNewCalculation);
    }

    private void HandleUseForNewCalculation()
    {
      _eventAggregator.GetEvent<UseForNewCalculationClicked>().Publish(true);
    }

    public DelegateCommand FindSafeWorkingDistanceCommand { get; set; }
    public DelegateCommand StandardWorkingDistanceCommand { get; set; }
    public DelegateCommand UseForNewCalculation { get; set; }


    private void FindSafeWorkingDistance()
    {
      _eventAggregator.GetEvent<OpenModalEvent>().Publish(Constants.FIND_SAFE_WORKING_DISTANCE_VIEW);
    }

    private void StandardWorkingDistance()
    {
      _eventAggregator.GetEvent<OpenModalEvent>().Publish(Constants.STANDARD_WORKING_DISTANCE_VIEW);
    }

    private void HandleCalculationOutputUpdated(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
      EstimatedArcFaultCurrentInAmps = CalculationOutput.EstimatedArcFaultCurrent * 1000;
      if (CalculationOutput.BoltedFaultCurrent.HasValue)
        BoltedFaultCurrentInAmps = CalculationOutput.BoltedFaultCurrent.Value * 1000;
    }

    private void InputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;

      ArcDurationCalculationMethodText = CalculationInput.IsArcDurationCalculated ? "CALCULATED" : "MANUAL";
      EquipmentEnclosure = calculationInput.IsOpenAir.HasValue && calculationInput.IsOpenAir.Value ? AppResources.Open_Air : AppResources.In_Box;
      BatteryEnclosure = calculationInput.InCabinet.HasValue && calculationInput.InCabinet.Value ? AppResources.In_Box : AppResources.Open_Air;

      VoltageConductor = calculationInput.HasCable.HasValue && calculationInput.HasCable.Value ? AppResources.Cable : AppResources.No_Cable;
      Grounding = calculationInput.IsSolidGround.HasValue && calculationInput.IsSolidGround.Value ? AppResources.Solidly_Grounded : AppResources.Ungrounded;
      ShowArcDurationCalculationFields = calculationInput.IsArcDurationCalculated;


      if (CalculationInput.IsAlternatingCurrent.HasValue)
      {
        List<VisibilityModel> visibilities = _showingAndHidingService.ShowingAndHidingService(calculationInput);
        foreach (VisibilityModel visibility in visibilities)
        {
          if (visibility.Parameter.Equals("ShowAlternatingCurrentFields"))
            ShowAlternatingCurrentFields = visibility.ParameterValue;
          if (visibility.Parameter.Equals("ShowDirectCurrentFields"))
            ShowDirectCurrentFields = visibility.ParameterValue;
          if (visibility.Parameter.Equals("ShowConductorFields"))
            ShowConductorFields = visibility.ParameterValue;
          if (visibility.Parameter.Equals("ShowTransformerFields"))
            ShowTransformerFields = visibility.ParameterValue;
        }

        IsDirectCurrent = !calculationInput.IsAlternatingCurrent.Value;
      }
      VoltageType = calculationInput.IsAlternatingCurrent.HasValue && calculationInput.IsAlternatingCurrent.Value ? "AC" : "DC";
      Transformer = calculationInput.HasTransformer.HasValue && calculationInput.HasTransformer.Value ? AppResources.Upstream_XFR : AppResources.No_Transformer;

    }

    private void CalculateClick(bool clicked)
    {
      if (clicked)
      {
        HasStandardWorkingDistance = !CalculationInput.HasCustomWorkingDistance;
        CalculationOutput = _calculatorService.Calculate(CalculationInput) as CalculationOutput;
        CalculationOutput.IncidentEnergy = Math.Round(CalculationOutput.IncidentEnergy, 2, MidpointRounding.ToEven);
        WorkingDistanceRounded = Math.Round(CalculationInput.WorkingDistance.Value, 2, MidpointRounding.ToEven);

        OverFortyCal = CalculationOutput.IncidentEnergy > 40.00m;
        IsLevelZero = CalculationOutput.IncidentEnergy <= 1.2m;
        IsLevelTwo = CalculationOutput.IncidentEnergy > 1.2m && CalculationOutput.IncidentEnergy <= 8;
        IsLevelFour = CalculationOutput.IncidentEnergy > 8 && CalculationOutput.IncidentEnergy <= 40;

        ShowFindSafeWorkingDistance = OverFortyCal || CalculationInput.HasCustomWorkingDistance;

        _eventAggregator.GetEvent<CalculationOutputUpdated>().Publish(CalculationOutput);
      }
    }
    
    /// <summary>/// Prism Property/// </summary>
		private CalculationOutput _calculationOutput;

    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set { SetProperty(ref _calculationOutput, value); }
    }

    private CalculationInput _calculationInput;

    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set { SetProperty(ref _calculationInput, value); }
    }

    /// <summary>/// Prism Property/// </summary>
		private decimal _sourceVoltage;

    public decimal SourceVoltage
    {
      get { return _sourceVoltage; }
      set { SetProperty(ref _sourceVoltage, value); }
    }

    private string _action;
    public string Action
    {
      get { return _action; }
      set { SetProperty(ref _action, value); }
    }

    /// <summary>/// Prism Property/// </summary>
    private string _equipmentEnclosure;
    public string EquipmentEnclosure
    {
      get { return _equipmentEnclosure; }
      set { SetProperty(ref _equipmentEnclosure, value); }
    }

    private string _batteryEnclosure;
    public string BatteryEnclosure
    {
      get { return _batteryEnclosure; }
      set { SetProperty(ref _batteryEnclosure, value); }
    }

    /// <summary>/// Prism Property/// </summary>
		private string _voltageConductor;

    public string VoltageConductor
    {
      get { return _voltageConductor; }
      set { SetProperty(ref _voltageConductor, value); }
    }

    /// <summary>/// Prism Property/// </summary>
    //private string _hasTransformer;

    // public string HasTransformer
    //{
    //  get { return _hasTransformer; }
    //  set { SetProperty(ref _hasTransformer, value); }
    //}

    /// <summary>/// Prism Property/// </summary>
    private string _grounding;

    public string Grounding
    {
      get { return _grounding; }
      set { SetProperty(ref _grounding, value); }
    }

    private bool _isDirectCurrent;

    public bool IsDirectCurrent
    {
      get { return _isDirectCurrent; }
      set { SetProperty(ref _isDirectCurrent, value); }
    }

    /// <summary>/// Prism Property/// </summary>
    private bool _showAlternatingCurrentFields;

    public bool ShowAlternatingCurrentFields
    {
      get { return _showAlternatingCurrentFields; }
      set { SetProperty(ref _showAlternatingCurrentFields, value); }
    }
    /// <summary>/// Prism Property/// </summary>
		private bool _showDirectCurrentFields;

    public bool ShowDirectCurrentFields
    {
      get { return _showDirectCurrentFields; }
      set { SetProperty(ref _showDirectCurrentFields, value); }
    }

    /// <summary>/// Prism Property/// </summary>
    private bool _showConductorFields;

    public bool ShowConductorFields
    {
      get { return _showConductorFields; }
      set { SetProperty(ref _showConductorFields, value); }
    }

    /// <summary>/// Prism Property/// </summary>
		private bool _showTransformerFields;

    public bool ShowTransformerFields
    {
      get { return _showTransformerFields; }
      set { SetProperty(ref _showTransformerFields, value); }
    }

    /// <summary>/// Prism Property/// </summary>
    private bool _showFindSafeWorkingDistance;

    public bool ShowFindSafeWorkingDistance
    {
      get { return _showFindSafeWorkingDistance; }
      set { SetProperty(ref _showFindSafeWorkingDistance, value); }
    }

    /// <summary>/// Prism Property/// </summary>
		private bool _showArcDurationCalculationFields;

    public bool ShowArcDurationCalculationFields
    {
      get { return _showArcDurationCalculationFields; }
      set { SetProperty(ref _showArcDurationCalculationFields, value); }
    }

    /// <summary>/// Prism Property/// </summary>
		private bool _overFortyCal;

    public bool OverFortyCal
    {
      get { return _overFortyCal; }
      set
      {
        SetProperty(ref _overFortyCal, value);
        NotOverFortyCal = !_overFortyCal;
      }
    }
    /// <summary>/// Prism Property/// </summary>
		private bool _notOverFortyCal;

    public bool NotOverFortyCal
    {
      get { return _notOverFortyCal; }
      set { SetProperty(ref _notOverFortyCal, value); }
    }

    /// <summary>/// Prism Property/// </summary>
    private bool _hasStandardWorkingDistance;

    public bool HasStandardWorkingDistance
    {
      get { return _hasStandardWorkingDistance; }
      set { SetProperty(ref _hasStandardWorkingDistance, value); }
    }

    private string _arcDurationCalculationMethodText;

    public string ArcDurationCalculationMethodText
    {
      get { return _arcDurationCalculationMethodText; }
      set { SetProperty(ref _arcDurationCalculationMethodText, value); }
    }

    private string _nonStandardWorkingDistanceSelected;

    public string NonStandardWorkingDistanceSelected
    {
      get { return _nonStandardWorkingDistanceSelected; }
      set { SetProperty(ref _nonStandardWorkingDistanceSelected, value); }
    }

    private decimal? _estimatedArcFaultCurrentInAmps;
    public decimal? EstimatedArcFaultCurrentInAmps
    {
      get { return _estimatedArcFaultCurrentInAmps; }
      set { SetProperty(ref _estimatedArcFaultCurrentInAmps, value); }
    }

    private decimal? _boltedFaultCurrentInAmps;
    public decimal? BoltedFaultCurrentInAmps
    {
      get { return _boltedFaultCurrentInAmps; }
      set { SetProperty(ref _boltedFaultCurrentInAmps, value); }
    }

    private decimal? _workingDistanceRounded;
    public decimal? WorkingDistanceRounded
    {
      get { return _workingDistanceRounded; }
      set { SetProperty(ref _workingDistanceRounded, value); }
    }

    private string _voltageType;
    public string VoltageType
    {
      get { return _voltageType; }
      set { SetProperty(ref _voltageType, value); }
    }
    private string _transformer;
    public string Transformer
    {
      get { return _transformer; }
      set { SetProperty(ref _transformer, value); }
    }

    private bool _isLevelZero;

    public bool IsLevelZero
    {
      get { return _isLevelZero; }
      set { SetProperty(ref _isLevelZero, value); }
    }

    private bool _isLevelTwo;

    public bool IsLevelTwo
    {
      get { return _isLevelTwo; }
      set { SetProperty(ref _isLevelTwo, value); }
    }

    private bool _isLevelFour;

    public bool IsLevelFour
    {
      get { return _isLevelFour; }
      set { SetProperty(ref _isLevelFour, value); }
    }
  }
}
