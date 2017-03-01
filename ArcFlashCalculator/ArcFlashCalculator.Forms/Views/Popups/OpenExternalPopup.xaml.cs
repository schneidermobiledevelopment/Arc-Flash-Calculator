using ArcFlashCalculator.Forms.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class OpenExternalPopup : PopupPage
  {

    private OpenExternalPopupViewModel ViewModel => this.BindingContext as OpenExternalPopupViewModel;
    public OpenExternalPopup(string parameter)
    {
      InitializeComponent();
      ViewModel.Parameter = parameter;
    }

    public OpenExternalPopup()
    {
      InitializeComponent();
    }
  }
}
