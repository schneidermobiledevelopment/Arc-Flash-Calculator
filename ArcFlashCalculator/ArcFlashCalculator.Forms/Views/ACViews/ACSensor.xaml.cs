using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class ACSensor : ContentPage
  {
    private ACSensorViewModel ViewModel => this.BindingContext as ACSensorViewModel;

    public ACSensor()
    {
      InitializeComponent();
      arcDuration.TextChanged += ArcDuration_TextChanged;
      sensorRating.TextChanged += SensorRating_TextChanged;

    }

    private async  void ArcDuration_TextChanged(object sender, TextChangedEventArgs e)
    {
     await  ViewModel.ArcDurationTextChangedCommand.Execute();
    }
    private async void SensorRating_TextChanged(object sender, TextChangedEventArgs e)
    {
      await ViewModel.SensorRatingTextChangedCommand.Execute();
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      if (ViewModel != null)
      {
        if (ViewModel.Field == "Sensor")
        {
          sensorRating.Focus = true;
        }
        else if (ViewModel.Field == "ArcDuration")
        {
          arcDuration.Focus = true;
        }
      }
    }
  }
}