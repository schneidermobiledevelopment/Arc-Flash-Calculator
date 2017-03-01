using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class About : ContentPage
  {
    public About()
    {
      InitializeComponent();
      NavigationPage.SetHasNavigationBar(this, false);
      var tapGestureRecognizer = new TapGestureRecognizer();
      tapGestureRecognizer.Tapped += (s, e) => {
        ((MasterDetailPage)this.Parent.Parent).IsPresented = true;  
      };
      hamburgerIcon.GestureRecognizers.Add(tapGestureRecognizer);
    }
    protected override bool OnBackButtonPressed()
    {
      return true;
    }
  }
}
