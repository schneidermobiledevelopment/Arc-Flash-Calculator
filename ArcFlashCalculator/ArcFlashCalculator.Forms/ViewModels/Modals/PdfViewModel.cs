using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ArcFlashCalculator.Forms.ViewModels
{
  public class PdfViewModel : BindableBase
  {
    private INavigationService _navigationService;
    private string page;

    public PdfViewModel(INavigationService navigationService)
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
