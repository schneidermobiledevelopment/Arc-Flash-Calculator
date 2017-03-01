using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class WorkingDistanceTableViewModel : BindableBase
  {
    private readonly INavigationService _navigationService;
    public WorkingDistanceTableViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService;
    }

    public DelegateCommand CloseCommand => new DelegateCommand(Close);
    private async void Close()
    {
      await _navigationService.GoBackAsync(null, true);
    }
  }
}
