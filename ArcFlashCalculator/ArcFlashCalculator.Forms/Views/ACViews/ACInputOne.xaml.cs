using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class ACInputOne : ContentPage
  {
    private ACInputOneViewModel ViewModel => this.BindingContext as ACInputOneViewModel;
    public ACInputOne()
    {
      InitializeComponent();
      if (Device.Idiom == TargetIdiom.Phone)
        if(Device.OS == TargetPlatform.iOS)
      {
        ungroundedButton.WidthRequest = 150;
        groundedButton.WidthRequest = 110;
      }
      else if(Device.OS == TargetPlatform.Android)
        {
          ungroundedButton.WidthRequest = 170;
        }
    }
    protected override void OnAppearing()
    {
      base.OnAppearing();
      if (ViewModel != null)
      {
        if (ViewModel.Field == "Nominal")
        {
          nominal.Focus = true;
        }
        else if (ViewModel.Field == "Source")
        {
          source.Focus = true;
        }
      }
    }
  }
}
