using System;
using System.Linq;
using ArcFlashCalculator.Core;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.WPF.Commands;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ArcFlashCalculator.WPF.ViewModels.Modals
{
  public class FindSafeWorkingDistanceViewModel : BindableBase
  {
    private readonly IDataService _dataService;
    private readonly IEventAggregator _eventAggregator;
    private readonly ICalculatorService _calculatorService;

    public FindSafeWorkingDistanceViewModel(IEventAggregator eventAggregator, IDataService dataService, ICalculatorService calculatorService)
    {
      //events
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(UpdateCalculationInput);
      _eventAggregator.GetEvent<CalculationOutputUpdated>().Subscribe(UpdateCalculationOutput);

      //data service
      _dataService = dataService;

      //calc service
      _calculatorService = calculatorService;

      //commands
      CloseModalCommand = new DelegateCommand(CloseModal);
      NewCalculationAt8 = new DelegateCommand(NewCalculationFor8Cal);
      NewCalculationAtOnePointTwo = new DelegateCommand(NewCalculationForOnePointTwoCal);
      NewCalculationAt40 = new DelegateCommand(NewCalculationFor40Cal);
      NewCalculationAtStandardWorkingDistance = new DelegateCommand(NewCalculationForStandardWorkingDistance);
    }


    public DelegateCommand CloseModalCommand { get; set; }
    public DelegateCommand NewCalculationAt8 { get; set; }
    public DelegateCommand NewCalculationAtOnePointTwo { get; set; }
    public DelegateCommand NewCalculationAt40 { get; set; }
    public DelegateCommand NewCalculationAtStandardWorkingDistance { get; set; }

    private CalculationInput _calculationInput;
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set { SetProperty(ref _calculationInput, value); }
    }

    /// <summary>/// Prism Property/// </summary>
		private CalculationOutput _calculationOutput;
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set { SetProperty(ref _calculationOutput, value); }
    }

    private decimal? _originalIncidentEnergy;
    public decimal? OriginalIncidentEnergy
    {
      get { return _originalIncidentEnergy; }
      set { SetProperty(ref _originalIncidentEnergy, value); }
    }

    private decimal? _originalWorkingDistance;
    public decimal? OriginalWorkingDistance
    {
      get { return _originalWorkingDistance; }
      set { SetProperty(ref _originalWorkingDistance, value); }
    }

    private void UpdateCalculationOutput(CalculationOutput calculationOutput)
    {
      CalculationOutput = calculationOutput;
      if (OriginalIncidentEnergy == null && CalculationOutput.IncidentEnergy > 0)
        OriginalIncidentEnergy = CalculationOutput.IncidentEnergy;
    }

    private void UpdateCalculationInput(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      if (OriginalWorkingDistance==null && CalculationInput.WorkingDistance.HasValue)
      {
        OriginalWorkingDistance = CalculationInput.WorkingDistance.Value;
      }
    }

    private void CloseModal()
    {
      _eventAggregator.GetEvent<CloseModalEvent>().Publish(Constants.FIND_SAFE_WORKING_DISTANCE_VIEW);
    }

    private void NewCalculationFor8Cal()
    {
      NewCalculation(CalculationOutput.MinimumWorkingDistance8Cal, true);
    }

    private void NewCalculationForOnePointTwoCal()
    {
      NewCalculation(CalculationOutput.MinimumWorkingDistanceOnePointTwoCal, true);
    }
    private void NewCalculationFor40Cal()
    {
      NewCalculation(CalculationOutput.MinimumWorkingDistance40Cal, true);
    }

    private void NewCalculationForStandardWorkingDistance()
    {
      CalculationInput.HasCustomWorkingDistance = false;
      if (OriginalWorkingDistance != null) NewCalculation(OriginalWorkingDistance.Value, false);
    }

    private void NewCalculation(decimal newWorkingDistance, bool hasCustomWorkingDistance)
    {
      CalculationInput.WorkingDistance = newWorkingDistance;
      CalculationInput.HasCustomWorkingDistance = hasCustomWorkingDistance;
      _eventAggregator.GetEvent<CalculateButtonClicked>().Publish(true);
      CloseModal();
    }
  }
}
