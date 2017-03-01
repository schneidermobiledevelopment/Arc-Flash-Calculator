using System;
using System.Collections.Generic;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ArcFlashCalculator.Core;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.WPF.Commands;
using Microsoft.Practices.Unity;

namespace ArcFlashCalculator.WPF.ViewModels
{
  public class FormViewModel : BindableBase
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IDataService _dataService;
    private readonly ICalculatorService _calculatorService;

    public FormViewModel(IEventAggregator eventAggregator, IDataService dataService, ICalculationInput calculationInput, ICalculationOutput calculationOutput, ICalculatorService calculatorService)
    {
      _dataService = dataService;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(UpdateCalculationInput);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(UpdateCalculationOutput);
      _calculationOutput = calculationOutput as CalculationOutput;
      _calculationInput = calculationInput as CalculationInput;    
      _calculatorService = calculatorService;

//#if DEBUG
//      CalculationInput.Personnel = "Stuart";
//      CalculationInput.Location = "Victoria";
//      CalculationInput.Action = "GIS Inspection";
//      CalculationInput.IsAlternatingCurrent = true;
//      CalculationInput.NominalVoltage = 15000;
//      CalculationInput.IsOpenAir = true;
//      CalculationInput.IsSolidGround = true;
//      CalculationInput.HasTransformer = true;
//      CalculationInput.PrimaryVoltage = 1000;
//      CalculationInput.XfmrImpedance = 5.75m;
//      CalculationInput.HasCable = true;
//      CalculationInput.ConductorLength = 100;
//      CalculationInput.ConductorPerPhase = 2;
//      CalculationInput.ConductorSizeId = 0;
//      CalculationInput.XfmrKVA = 1234;

//      CalculationInput.SourceFaultCurrent = 1000;
//      CalculationInput.EquipmentTypeId = 0;
//      CalculationInput.ArcDurationValue = 2;
//#endif

      FindEquipmentTripCurvesCommand = new DelegateCommand(FindEquipmentTripCurves);
      CalculateArcDurationCommand = new DelegateCommand(CalculateArcDuration);
    }

   

    private void FindEquipmentTripCurves()
    {
      _eventAggregator.GetEvent<OpenModalEvent>().Publish(Constants.FIND_EQUIPMENT_TRIP_CURVES_VIEW);
    }

    public DelegateCommand FindEquipmentTripCurvesCommand { get; set; }
    public DelegateCommand CalculateArcDurationCommand { get; set; }

    public ItemListViewModel<Manufacturer> Manufacturers { get; set; }
    public ItemListViewModel<BreakerStyle> BreakerStyles { get; set; }
    public ItemListViewModel<TripUnitType> TripUnitTypes { get; set; }
    public ItemListViewModel<LongTimeDelay> LongTimeDelays { get; set; }
    public ItemListViewModel<ShortTimePickup> ShortTimePickups { get; set; }
    public ItemListViewModel<ShortTimeDelay> ShortTimeDelays { get; set; }
    public ItemListViewModel<Instantaneous> Instantaneous { get; set; }

    
    private void CalculateArcDuration()
    {
      CalculationInput.IsArcDurationCalculated = true;
      _eventAggregator.GetEvent<OpenModalEvent>().Publish(Constants.ARCDURATION_CALCULATION_VIEW);
    }
   
    private void UpdateCalculationInput(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      ErrorCount = CalculationInput.ErrorCount;
      if (!CalculationInput.IsAlternatingCurrent.HasValue)
      {
        HasEstimatedArcFaultCurrent = false;
        ShowAlternatingCurrentFields = CalculationInput.IsAlternatingCurrent ?? false;
        ShowDirectCurrentFields = !CalculationInput.IsAlternatingCurrent ?? false;
        ShowConductorFields = CalculationInput.HasCable ?? false;
        ShowTransformerFields = CalculationInput.HasTransformer ?? false;
        
        if (Manufacturers!=null)
        {
          Manufacturers.SelectedItem = null;
        }
        if (BreakerStyles != null)
        {
          BreakerStyles.SelectedItem = null;
        }
        if (TripUnitTypes != null)
        {
          TripUnitTypes.SelectedItem = null;
        }      
      }
      else
      {
        IsReset = true;
        EnableCableFields = CalculationInput.HasCable.HasValue ? CalculationInput.HasCable.Value : false;
        EnableTransformerFields = CalculationInput.HasTransformer.HasValue ? CalculationInput.HasTransformer.Value : false;
        HasRangeErrors = calculationInput.ErrorCount > 0;
        HasErrors = !HasRangeErrors && calculationInput.HasErrors;
        IsDirectCurrent = !CalculationInput.IsAlternatingCurrent ?? false;
        ShowConductorFields = false;
        ShowTransformerFields = false;
        ShowAlternatingCurrentFields = calculationInput.IsAlternatingCurrent ?? false;
        ShowDirectCurrentFields = !calculationInput.IsAlternatingCurrent ?? false;
        if (ShowAlternatingCurrentFields)
        {
          ShowConductorFields = calculationInput.HasCable ?? false;
          ShowTransformerFields = calculationInput.HasTransformer ?? false;
        }

        //do output calc
        CalculationOutput.EstimatedArcFaultCurrent = _calculatorService.CalculateArcFaultCurrentAC(calculationInput);
        EstimatedArcFaultCurrentInAmps = CalculationOutput.EstimatedArcFaultCurrent*1000;
        HasEstimatedArcFaultCurrent = EstimatedArcFaultCurrentInAmps!=null;
        CalculationOutput.MultipleOfSensorRating = _calculatorService.CalculateMultipleSensorRatingAC(calculationInput, CalculationOutput.EstimatedArcFaultCurrent);
        _eventAggregator.GetEvent<CalculationOutputUpdated>().Publish(CalculationOutput);
      }
    }

    private void UpdateCalculationOutput(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
    }

    private string _toolTipText = "";
    public string ToolTipText
    {
      get { return _toolTipText; }
      set { SetProperty(ref _toolTipText, value); }
    }

		private Thickness _leftToolTipMargin;
    public Thickness LeftToolTipMargin
    {
      get { return _leftToolTipMargin; }
      set { SetProperty(ref _leftToolTipMargin, value); }
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

    private CalculationOutput _calculationOutput;
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set
      {
        SetProperty(ref _calculationOutput, value);
      }
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
		private bool _isReset;

    public bool IsReset
    {
      get { return _isReset; }
      set { SetProperty(ref _isReset, value); }
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

    private bool _enableCableFields;
    public bool EnableCableFields
    {
      get { return _enableCableFields; }
      set
      {
        SetProperty(ref _enableCableFields, value);
      }
    }

    private bool _enableTransformerFields;
    public bool EnableTransformerFields
    {
      get { return _enableTransformerFields; }
      set
      {
        SetProperty(ref _enableTransformerFields, value);
      }
    }

    /// <summary>/// Prism Property/// </summary>
		private bool _arcDurationEnabled = true;
    public bool ArcDurationEnabled
    {
      get { return _arcDurationEnabled; }
      set
      {
        SetProperty(ref _arcDurationEnabled, value);
      }
    }

    /// <summary>/// Prism Property/// </summary>
		private string _arcDurationString;
    public string ArcDurationString
    {
      get { return _arcDurationString; }
      set
      {
        SetProperty(ref _arcDurationString, value);
      }
    }

   

    public ObservableCollection<UnitOfMeasure> UnitOfMeasures => new ObservableCollection<UnitOfMeasure>(_dataService.GetUnitOfMeasure());
    public ObservableCollection<ConductorSize> ConductorSizes => new ObservableCollection<ConductorSize>(_dataService.GetConductorSizes());
    public ObservableCollection<EquipmentType> EquipmentTypes => new ObservableCollection<EquipmentType>(_dataService.GetEquipmentTypes());

    private bool _hasErrors = true;
    public bool HasErrors
    {
      get { return _hasErrors; }
      set { SetProperty(ref _hasErrors, value); }
    }

    private bool _hasRangeErrors;
    public bool HasRangeErrors
    {
      get { return _hasRangeErrors; }
      set { SetProperty(ref _hasRangeErrors, value); }
    }

		private int _errorCount;

    public int ErrorCount
    {
      get { return _errorCount; }
      set { SetProperty(ref _errorCount, value); }
    }

    private decimal? _estimatedArcFaultCurrentInAmps;
    public decimal? EstimatedArcFaultCurrentInAmps
    {
      get { return _estimatedArcFaultCurrentInAmps; }
      set { SetProperty(ref _estimatedArcFaultCurrentInAmps, value); }
    }

    private bool _hasEstimatedArcFaultCurrent;
    public bool HasEstimatedArcFaultCurrent
    {
      get { return _hasEstimatedArcFaultCurrent; }
      set { SetProperty(ref _hasEstimatedArcFaultCurrent, value); }
    }

    private DateTime _startDate = DateTime.Today;
    public DateTime StartDate
    {
      get { return _startDate; }
      set { SetProperty(ref _startDate, value); }
    }
  }
}
