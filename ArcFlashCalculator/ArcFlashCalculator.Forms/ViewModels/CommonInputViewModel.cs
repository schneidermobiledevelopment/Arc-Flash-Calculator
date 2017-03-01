using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class CommonInputViewModel : BindableBase, INavigationAware
  {
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;
    private readonly IUnityContainer _container;
    private CalculationInput _calculationInput;
    private readonly ICalculatorService _calculatorService;
    private readonly IEventAggregator _eventAggregator;



    public CommonInputViewModel(INavigationService navigationService, IUnityContainer container,IDataService dataService, ICalculationInput calculationInput, IEventAggregator eventAggregator, ICalculatorService calculatorService)
    {
      _navigationService = navigationService;
      _dataService = dataService;
      _calculationInput = calculationInput as CalculationInput;
      _container = container;
      _calculatorService = calculatorService;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);

#if DEBUG
      CalculationInput.IsAlternatingCurrent = false;
      CalculationInput.MaximumShortCircuitAvailable = 15000;
      CalculationInput.VoltageOfBattery = 123;
      CalculationInput.InCabinet = false;
#endif

    }

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
      IsEnabled = calculationInput.IsAlternatingCurrent.HasValue;
    }

    public DateTime MinimumDate { get { return DateTime.Now;} }

    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      {
        SetProperty(ref _calculationInput, value);
      }
    }

    private bool _isClicked = false ;
    private bool _isEnabled;
    public bool IsEnabled
    {
      get {
        return _isEnabled;
      }
      set {
        SetProperty(ref _isEnabled, value);
      }
    }

    private string _field;
    public string Field
    {
      get { return _field; }
      set { SetProperty(ref _field, value); }
    }

    public DelegateCommand ResetCommand => new DelegateCommand(Reset);

    private void Reset()
    {
      CalculationInput.Personnel = null;
      CalculationInput.Location = null;
      CalculationInput.Action = null;
      CalculationInput.CalculationDate = DateTime.Now;
      CalculationInput.IsAlternatingCurrent = null;
    }

    public DelegateCommand NextCommand => new DelegateCommand(Next, CanNavigate);

    private bool CanNavigate()
    {
      return (!_isClicked && CalculationInput.IsAlternatingCurrent.HasValue);
    }

    private async void Next()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("calculationInput", _calculationInput);
        if(CalculationInput.IsAlternatingCurrent.Value)
          await _navigationService.NavigateAsync("ACInputOne",p);
        else
          await _navigationService.NavigateAsync("DCInput",p);
      _isClicked = false;
    }

    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver,CanStartOver);

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

    public DelegateCommand HelpCommand => new DelegateCommand(Help,CanHelp);

    private bool CanHelp()
    {
      return !_isClicked;
    }

    private async void Help()
    {
      _isClicked = true;
        var p = new NavigationParameters();
        p.Add("help", "CommonInput");
      await _navigationService.NavigateAsync("NavPage/TechnicalHelp", p, true);
      _isClicked = false;
    }

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);

    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
     // if (parameters.ContainsKey("calculationInput"))
     //   HandleCalculationInputUpdated((CalculationInput) parameters["calculationInput"]);
      if (parameters.ContainsKey("Field"))
        Field = (string) parameters["Field"];
    }
  }
}
