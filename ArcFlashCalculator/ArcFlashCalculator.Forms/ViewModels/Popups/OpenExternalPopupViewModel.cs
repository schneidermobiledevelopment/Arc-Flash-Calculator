using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class OpenExternalPopupViewModel : BindableBase
  {
    private bool _isClicked;
    private readonly INavigationService _navigationService;
    public OpenExternalPopupViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService;
    }
    public DelegateCommand CancelCommand => new DelegateCommand(Cancel, CanExecute);
    public DelegateCommand OkCommand => new DelegateCommand(Ok, CanExecute);
    private bool CanExecute()
    {
      return !_isClicked;
    }
    private string _parameter;
    public string Parameter
    {
      get { return _parameter; }
      set { SetProperty(ref _parameter, value); }
    }
    private async void Cancel()
    {
      await PopupNavigation.PopAllAsync(true);
    }

    private async void Ok()
    {
      _isClicked = true;
      var url = GetUrl();
      Device.OpenUri(new Uri(url));
      await PopupNavigation.PopAllAsync(true);
      _isClicked = false;
    }

    private string GetUrl()
    {
      var url = "";
      switch (Parameter)
      {
        case "SquareD":
          url= "http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
          break;
        case "Eaton":
          url = "http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
          break;
        case "GE":
          url = "http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
          break;
        case "Siemens":
          url = "http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
          break;
        case "ABB":
          url = "http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
          break;
        case "Toshiba":
          url = "http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
          break;
        default :
          url = "http://products.schneider-electric.us/technical-library/?event=type.list&docType=sqd_trip_curve";
          break;
      }
      return url;
    }
  }
}
