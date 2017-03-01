using System;
using ArcFlashCalculator.Forms.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class TripCurvesViewModel : BindableBase
  {
    private bool _isClicked;
    private readonly INavigationService _navigationService;
    public TripCurvesViewModel( INavigationService navigationService)
    {
      _navigationService = navigationService;
    }


    private bool CanClick()
    {
      return !_isClicked;
    }
    public DelegateCommand CloseCommand => new DelegateCommand(Close, CanClick);
    private async void Close()
    {
      _isClicked = true;
      await _navigationService.GoBackAsync(null, true);
      _isClicked = false;
    }

    public DelegateCommand SquareDCommand => new DelegateCommand(SquareD , CanClick);

   

    private async void SquareD()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new OpenExternalPopup("SquareD"));
      _isClicked = false;
    }
    public DelegateCommand EatonCommand => new DelegateCommand(Eaton, CanClick);

    private async void Eaton()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new OpenExternalPopup("Eaton"));
      _isClicked = false;
    }
    public DelegateCommand GECommand => new DelegateCommand(GE, CanClick);

    private async void GE()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new OpenExternalPopup("GE"));
      _isClicked = false;
    }
    public DelegateCommand SiemensCommand => new DelegateCommand(Siemens, CanClick);

    private async void Siemens()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new OpenExternalPopup("Siemens"));
      _isClicked = false;
    }
    public DelegateCommand ABBCommand => new DelegateCommand(ABB, CanClick);

    private async void ABB()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new OpenExternalPopup("ABB"));
      _isClicked = false;
    }
    public DelegateCommand ToshibaCommand => new DelegateCommand(Toshiba, CanClick);

    private async void Toshiba()
    {
      _isClicked = true;
      await PopupNavigation.PushAsync(new OpenExternalPopup("Toshiba"));
      _isClicked = false;
    }

  }
}
