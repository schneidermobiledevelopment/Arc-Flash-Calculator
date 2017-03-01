using System;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Forms.Views;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class DCCircuitDiagramViewModel : BindableBase, INavigationAware
  {
    private INavigationService _navigationService;
    private readonly IUnityContainer _container;
    private CalculationInput _calculationInput;
    private readonly IEventAggregator _eventAggregator;
    private bool _isClicked = false;

    public DCCircuitDiagramViewModel(INavigationService navigationService, IUnityContainer container, ICalculationInput calculationInput, IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _calculationInput = calculationInput as CalculationInput;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated);
      _container = container;
    }

    private void HandleCalculationInputUpdated(CalculationInput calculationInput)
    {
      CalculationInput = calculationInput;
    }

    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      {
        SetProperty(ref _calculationInput, value);
      }
    }

    public DelegateCommand BackCommand => new DelegateCommand(Back);

    private async void Back()
    {
      await _navigationService.GoBackAsync();
    }

    public DelegateCommand NextCommand => new DelegateCommand(Next, CanNavigate);

    private bool CanNavigate()
    {
      return (!_isClicked);
    }

    private async void Next()
    {
      _isClicked = true;
      var p = new NavigationParameters();
      p.Add("calculationInput", _calculationInput);
      await _navigationService.NavigateAsync("ParameterSummary", p);
      _isClicked = false;
    }

    public DelegateCommand StartOverCommand => new DelegateCommand(StartOver, CanStartOver);

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

    public void OnNavigatedFrom(NavigationParameters parameters)
    {
      _eventAggregator.GetEvent<CalculationInputUpdated>().Publish(CalculationInput);
    }

    public void OnNavigatedTo(NavigationParameters parameters)
    {
     // if (parameters.ContainsKey("calculationInput"))
     //   CalculationInput = (CalculationInput)parameters["calculationInput"];
    }
  }
}
