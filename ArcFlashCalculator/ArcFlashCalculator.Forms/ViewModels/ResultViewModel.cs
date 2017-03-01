using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ResultViewModel : BindableBase, INavigationAware
  {
    private INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IValidator<CalculationInput> _validator;
    private readonly IUnityContainer _container;
    private CalculationInput _calculationInput;
    private ICalculatorService _calculatorService;
    private CalculationOutput _calculationOutput;
    private bool _isClicked;

    public ResultViewModel(INavigationService navigationService, IUnityContainer container, IDataService dataService, ICalculationInput calculationInput, IValidator<CalculationInput> validator,ICalculatorService calculatorService)
    {
      _navigationService = navigationService;
      _dataService = dataService;
      _calculationInput = calculationInput as CalculationInput;
      _validator = validator;
      _container = container;
      _calculatorService = calculatorService;
    }

    private ObservableCollection<ResultItem> _result;
    public ObservableCollection<ResultItem> Result
    {
      get { return _result; }
      set { SetProperty(ref _result, value); }
    }

    private ObservableCollection<TemplateSelector> _carouselPages;
    public ObservableCollection<TemplateSelector> CarouselPages
    {
      get { return _carouselPages; }
      set { SetProperty(ref _carouselPages, value); }
    }

    private List<int> _itemSource;
    public List<int> ItemSource
    {
      get { return _itemSource; }
      set { SetProperty(ref _itemSource, value); }
    }

    public void initCarousel()
    {
      CarouselPages = new ObservableCollection<TemplateSelector>()
      {
        new TemplateSelector() { Type = new DataTemplate(typeof(ACNormalLevel)) },
        new TemplateSelector() { Type = new DataTemplate(typeof(ACCalculationResult)) },
        new TemplateSelector() { Type = new DataTemplate(typeof(ACDataUser)) }
      };
    }

    public void InitList()
    {
      Result = new ObservableCollection<ResultItem>
      {
        new ResultItem { Title = "Personnel", Description=_calculationInput.Personnel},
        new ResultItem { Title = "Location", Description = _calculationInput.Location},
        new ResultItem { Title = "Action", Description = _calculationInput.Action},
        new ResultItem { Title = "Date", Description = _calculationInput.CalculationDate.ToString("dddd, MMMM d, yyyy" )},
        new ResultItem { Title = "Voltage Type", Description = "DC" },
        new ResultItem { Title = "Maximum Available Short Circuit", Description = CalculationInput.MaximumShortCircuitAvailable.Value.ToString() }
      };
      
        if (CalculationInput.InCabinet.Value)
          Result.Add(new ResultItem { Title = "Battery Enclosure?", Description = "In Cabinet" });
        else
          Result.Add(new ResultItem { Title = "Battery Enclosure?", Description = "Open Air" });
        Result.Add(new ResultItem { Title = "Battery String Voltage (volts)", Description = CalculationInput.VoltageOfBattery.Value.ToString() });
      Result.Add(new ResultItem { Title = "Incident Energy", Description = CalculationOutput.IncidentEnergy.ToString() });
      Result.Add(new ResultItem { Title = "Hazard/Risk Category", Description = CalculationOutput.HazardCat.ToString() });
      Result.Add(new ResultItem { Title = "Arc Flash Boundary", Description = CalculationOutput.ArcFlashBoundary.ToString() });
      Result.Add(new ResultItem { Title = "Incident Energy at Less than 50 volts", Description = CalculationOutput.IE50v.ToString() });
    }

    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set { SetProperty(ref _calculationInput, value); }
    }
    public CalculationOutput CalculationOutput
    {
      get { return _calculationOutput; }
      set { SetProperty(ref _calculationOutput, value); }
    }

    public DelegateCommand BackCommand => new DelegateCommand(Back);

    private async void Back()
    {
      await _navigationService.GoBackAsync();
    }

    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver);

    private async void StartOver()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new StartOverPopup());
      _isClicked = false;
    }

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
     
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {

      if (parameters.ContainsKey("calculationInput"))
      {
        CalculationInput = (CalculationInput)parameters["calculationInput"];
        CalculationOutput = (CalculationOutput) _calculatorService.Calculate(_calculationInput);
        InitList();
        initCarousel();
        ItemSource= new List<int>();
      }
    }
  }
  public class ResultItem
  {
    public string Title { get; set; }
    public string Description { get; set; }
  }

  public class TemplateSelector : DataTemplateSelector
  {

    public DataTemplate Type { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {  
          return new DataTemplate(typeof(ACCalculationResult)); 
    }
  }
}
