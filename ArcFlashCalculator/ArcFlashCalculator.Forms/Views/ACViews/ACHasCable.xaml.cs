using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class ACHasCable : ContentPage
  {
    private ACHasCableViewModel ViewModel => this.BindingContext as ACHasCableViewModel;
    public ACHasCable()
    {
      InitializeComponent();
    }
    protected override void OnAppearing()
    {
      base.OnAppearing();
      if (ViewModel != null)
      {
        if (ViewModel.Field == "ConductorPerPhase")
        {
          conductorPerPhase.Focus = true;
        }
        else if (ViewModel.Field == "ConductorLenght")
        {
          conductorLength.Focus = true;
        }
      }
    }
  }
}
