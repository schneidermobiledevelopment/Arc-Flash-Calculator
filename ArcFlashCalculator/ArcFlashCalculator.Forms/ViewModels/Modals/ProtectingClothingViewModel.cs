using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class ProtectingClothingViewModel : BindableBase
  {
    private readonly INavigationService _navigationService;

    public ProtectingClothingViewModel(INavigationService navigationService)
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