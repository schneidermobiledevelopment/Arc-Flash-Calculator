using System.Linq;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.WPF.Controls;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using ArcFlashCalculator.Core.Interfaces;
using System.Collections.Generic;
using ArcFlashCalculator.Core.Locals;

namespace ArcFlashCalculator.WPF.ViewModels
{
  public class ConfirmationViewModel : BindableBase
  {
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private readonly IShowingAndHidingService _showingAndHidingService;

    public ConfirmationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IShowingAndHidingService showingAndHidingService)
    {
      _regionManager = regionManager;
      _regionManager.RegisterViewWithRegion("CircuitDiagramRegion", typeof(DiagramView));
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(HandleCalculationOutputUpdated);
      _showingAndHidingService = showingAndHidingService;
    }

    private void HandleCalculationOutputUpdated(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
      EstimatedArcFaultCurrentInAmps = CalculationOutput.EstimatedArcFaultCurrent*1000;
    }

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      if (calculationInput.IsAlternatingCurrent.HasValue && calculationInput.IsAlternatingCurrent.Value)
      {
        VoltageType = "AC";
        EquipmentEnclosure = calculationInput.IsOpenAir.HasValue && calculationInput.IsOpenAir.Value ? AppResources.Open_Air : AppResources.In_Box;
        VoltageConductor = calculationInput.HasCable.HasValue && calculationInput.HasCable.Value ? AppResources.Cable : AppResources.No_Cable;
        Transformer = calculationInput.HasTransformer.HasValue && calculationInput.HasTransformer.Value ? AppResources.Upstream_XFR : AppResources.No_Transformer;
        Grounding = calculationInput.IsSolidGround.HasValue && calculationInput.IsSolidGround.Value ? AppResources.Solidly_Grounded : AppResources.Ungrounded;

        ShowArcDurationCalculationFields = calculationInput.IsArcDurationCalculated;
      }
      else
      {
        VoltageType = "DC";
        BatteryEnclosure = calculationInput.InCabinet.HasValue && calculationInput.InCabinet.Value ? "In Box" : "Open Air";
      }

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
      }
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

    private string _voltageConductor;
    public string VoltageConductor
    {
      get { return _voltageConductor; }
      set { SetProperty(ref _voltageConductor, value); }
    }

    private string _transformer;
    public string Transformer
    {
      get { return _transformer; }
      set { SetProperty(ref _transformer, value); }
    }

    private string _grounding;
    public string Grounding
    {
      get { return _grounding; }
      set { SetProperty(ref _grounding, value); }
    }

    private string _voltageType;
    public string VoltageType
    {
      get { return _voltageType; }
      set { SetProperty(ref _voltageType, value); }
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
		private bool _showArcDurationCalculationFields;

    public bool ShowArcDurationCalculationFields
    {
      get { return _showArcDurationCalculationFields; }
      set { SetProperty(ref _showArcDurationCalculationFields, value); }
    }

    private decimal? _estimatedArcFaultCurrentInAmps;
    public decimal? EstimatedArcFaultCurrentInAmps
    {
      get { return _estimatedArcFaultCurrentInAmps; }
      set { SetProperty(ref _estimatedArcFaultCurrentInAmps, value); }
    }
  }
}
