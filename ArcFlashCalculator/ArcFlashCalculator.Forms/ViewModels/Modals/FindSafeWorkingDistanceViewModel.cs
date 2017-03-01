using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Model;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class FindSafeWorkingDistanceViewModel : BindableBase, INavigationAware
  {

    public DelegateCommand NewCalculationAt8 => new DelegateCommand(NewCalculationFor8Cal);
    public DelegateCommand NewCalculationAtOnePointTwo => new DelegateCommand(NewCalculationForOnePointTwoCal);
    public DelegateCommand NewCalculationAt40 => new DelegateCommand(NewCalculationFor40Cal);
    public DelegateCommand NewCalculationAtStandardWorkingDistance => new DelegateCommand(NewCalculationForStandardWorkingDistance);
    private readonly INavigationService _navigationService;
    private readonly IEventAggregator _eventAggregator;


    public FindSafeWorkingDistanceViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _eventAggregator = eventAggregator;
    }

    private ObservableCollection<WorkingDistance> _workingDistances;
    public ObservableCollection<WorkingDistance> WorkingDistances
    {
      get { return _workingDistances; }
      set { SetProperty(ref _workingDistances, value); }
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
    private decimal? _originalWorkingDistance;
    public decimal? OriginalWorkingDistance
    {
      get { return _originalWorkingDistance; }
      set { SetProperty(ref _originalWorkingDistance, value); }
    }

    private decimal? _originalIncidentEnergy;
    public decimal? OriginalIncidentEnergy
    {
      get { return _originalIncidentEnergy; }
      set { SetProperty(ref _originalIncidentEnergy, value); }
    }


    public void InitWorkingDistances()
    {
      WorkingDistances= new ObservableCollection<WorkingDistance>();
      if (CalculationInput.WorkingDistance!= CalculationOutput.MinimumWorkingDistanceOnePointTwoCal)
        WorkingDistances.Add(new WorkingDistance() {Category ="0",MinimumWorkingDistance = CalculationOutput.MinimumWorkingDistanceOnePointTwoCal, MaximumIncidentEnergy = "1.2 Cal/cm²", BackgroundColor = Color.FromHex("#339152"), Command = NewCalculationAtOnePointTwo});
      if (CalculationInput.WorkingDistance != CalculationOutput.MinimumWorkingDistance8Cal)
        WorkingDistances.Add(new WorkingDistance() { Category = "2", MinimumWorkingDistance = CalculationOutput.MinimumWorkingDistance8Cal, MaximumIncidentEnergy = "8 Cal/cm²", BackgroundColor = Color.FromHex("#339152"), Command = NewCalculationAt8});
      if (CalculationInput.WorkingDistance != CalculationOutput.MinimumWorkingDistance40Cal)
        WorkingDistances.Add(new WorkingDistance() { Category = "4", MinimumWorkingDistance = CalculationOutput.MinimumWorkingDistance40Cal, MaximumIncidentEnergy = "40 Cal/cm²", BackgroundColor = Color.FromHex("#339152"), Command = NewCalculationAt40});
      if (CalculationInput.HasCustomWorkingDistance)
        WorkingDistances.Add(new WorkingDistance() { Category = "Danger", MinimumWorkingDistance =  OriginalWorkingDistance.Value, MaximumIncidentEnergy = $"{OriginalIncidentEnergy.Value:N0 Cal/cm²}", BackgroundColor = Color.Red , Command = NewCalculationAtStandardWorkingDistance });
    }

    private void NewCalculationFor8Cal()
    {
      NewCalculation(CalculationOutput.MinimumWorkingDistance8Cal,true);
    }

    private void NewCalculationForOnePointTwoCal()
    {
      NewCalculation(CalculationOutput.MinimumWorkingDistanceOnePointTwoCal,true);
    }
    private void NewCalculationFor40Cal()
    {
      NewCalculation(CalculationOutput.MinimumWorkingDistance40Cal,true);
    }
    private void NewCalculationForStandardWorkingDistance()
    {
      CalculationInput.HasCustomWorkingDistance = false;
      NewCalculation(OriginalWorkingDistance.Value, false);
    }

    private void NewCalculation(decimal newWorkingDistance, bool hasCustomWorkingDistance)
      {
      CalculationInput.WorkingDistance = newWorkingDistance;
      CalculationInput.HasCustomWorkingDistance = hasCustomWorkingDistance;
      var p = new NavigationParameters();
      p.Add("calculationInput", CalculationInput);
      _navigationService.GoBackAsync(p, true);
    }

    public DelegateCommand CloseCommand => new DelegateCommand(Close);
    private async void Close()
    {
      await _navigationService.GoBackAsync(null, true);
    }

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
      CalculationInput = (CalculationInput)parameters["calculationInput"];
      CalculationOutput = (CalculationOutput) parameters["calculationOutput"];
      OriginalWorkingDistance = (decimal?)parameters["OriginalWorkingDistance"];
      OriginalIncidentEnergy = (decimal?)parameters["OriginalIncidentEnergy"];
      InitWorkingDistances();
    }
  }

  public class WorkingDistance
  {
    public string Category { get; set; }
    public string MaximumIncidentEnergy { get; set; }
    public decimal MinimumWorkingDistance{ get; set; }
    public Color BackgroundColor { get; set; }
    public DelegateCommand Command { get; set; }
  }
}
