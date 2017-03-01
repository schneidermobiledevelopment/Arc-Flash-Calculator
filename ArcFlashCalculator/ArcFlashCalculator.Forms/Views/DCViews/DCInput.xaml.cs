using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class DCInput : ContentPage
  {
    private DCInputViewModel ViewModel => this.BindingContext as DCInputViewModel;
    public DCInput()
    {
      InitializeComponent();
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      if (ViewModel != null)
      {
        if (ViewModel.Field == "MaximumAvailableShortCircuit")
        {
          maximumShortCircuitAvailable.Focus = true;
        }
        else if (ViewModel.Field == "BatteryStringVoltage")
        {
          VoltageOfBattery.Focus = true;
        }
      }
    }
  }
}
