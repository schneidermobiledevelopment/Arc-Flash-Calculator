using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Forms.Views;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class StartOverPopupViewModel : BindableBase
  {
    private readonly INavigationService _navigationService;
    private readonly IUnityContainer _container;
    private CalculationInput _calculationInput;
    private bool _isClicked = false;
    public StartOverPopupViewModel(INavigationService navigationService, IUnityContainer container, ICalculationInput calculationInput)
    {
      _navigationService = navigationService;
      _calculationInput = calculationInput as CalculationInput;
      _container = container;
    }

    public DelegateCommand CancelCommand => new DelegateCommand(Cancel, CanExecute);

    private async void  Cancel()
    {
      await PopupNavigation.PopAllAsync();
    }
    public CalculationInput CalculationInput
    {
      get { return _calculationInput; }
      set
      {
        SetProperty(ref _calculationInput, value);
      }
    }
    public DelegateCommand OkCommand => new DelegateCommand(Ok, CanExecute);

    private bool CanExecute()
    {
      return !_isClicked;
    }

    private async void Ok()
    {
      _isClicked = true;
      CalculationInput = _container.Resolve<ICalculationInput>() as CalculationInput;
      await _navigationService.NavigateAsync("app:///MasterPage/NavPage/CommonInput");
      await PopupNavigation.PopAllAsync(true);
      _isClicked = false;
    }
  }
}
