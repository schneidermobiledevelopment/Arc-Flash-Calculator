using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using ArcFlashCalculator.Forms.Views;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ACCircuitDiagramViewModel : BindableBase, INavigationAware
    {
    private INavigationService _navigationService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IUnityContainer _container;
      private bool _isClicked;

    #region Constructor
    public ACCircuitDiagramViewModel(INavigationService navigationService, ICalculationInput calculationInput, IUnityContainer container,IEventAggregator eventAggregator)
    {
      _navigationService = navigationService;
      _calculationInput = calculationInput as CalculationInput;
      _container = container;
      _eventAggregator = eventAggregator;
      _eventAggregator.GetEvent<CalculationInputUpdated>().Subscribe(HandleCalculationInputUpdated, keepSubscriberReferenceAlive: true);

    }
    #endregion

      private void HandleCalculationInputUpdated(CalculationInput calculationInput)
      {
        CalculationInput = calculationInput;
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
    public DelegateCommand BackCommand => new DelegateCommand(Back);

    private void Back()
    {
      _navigationService.GoBackAsync();
    }

    public DelegateCommand NextCommand => new DelegateCommand(Next);

    private async void Next()
    {
      var p = new NavigationParameters();
      p.Add("calculationInput", CalculationInput);
      await _navigationService.NavigateAsync("ParameterSummary",p);
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
      //if (parameters.ContainsKey("calculationInput"))
      //  CalculationInput = (CalculationInput)parameters["calculationInput"];
    }
  }
}
