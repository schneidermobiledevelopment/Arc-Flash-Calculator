using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class ACCableAndXfmr : ContentPage
  {
    private ACCableAndXfmrViewModel ViewModel => this.BindingContext as ACCableAndXfmrViewModel;
    public ACCableAndXfmr()
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
        if (ViewModel.Field == "PrimaryVoltage")
        {
          primaryVoltage.Focus = true;
        }
        else if (ViewModel.Field == "XfmrImpedance")
        {
          xmfrImpedance.Focus = true;
        }
        else if (ViewModel.Field == "XfmrKVA")
        {
          xfmrKVA.Focus = true;
        }
      }
    }
  }
}
