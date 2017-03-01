using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;


namespace ArcFlashCalculator.Forms.Views
{
  public partial class CommonInput : ContentPage
  {
    private CommonInputViewModel ViewModel => this.BindingContext as CommonInputViewModel;

    public CommonInput()
    {
      InitializeComponent();

    }
    protected override bool OnBackButtonPressed()
    {
      return true;
    }
    protected override void OnAppearing()
    {
      base.OnAppearing();
      if (ViewModel != null)
      {
        if (ViewModel.Field == "Personnel")
        {
          personnel.Focus = true;
        }
        else if (ViewModel.Field == "Location")
        {
          location.Focus = true;
        }
        else if(ViewModel.Field == "Action")
        {
          action.Focus = true;
        }
      }
    }
  }
}