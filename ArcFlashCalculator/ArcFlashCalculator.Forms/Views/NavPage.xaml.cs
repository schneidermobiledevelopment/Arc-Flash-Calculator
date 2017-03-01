using Prism.Navigation;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class NavPage : NavigationPage, INavigationPageOptions
  {
    public NavPage()
    {
      InitializeComponent();
    }

    public bool ClearNavigationStackOnNavigation
    {
      get { return true; }
    }
  }
}
