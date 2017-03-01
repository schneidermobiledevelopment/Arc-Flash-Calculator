using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class ACInputTwo : ContentPage
  {
    private ACInputTwoViewModel ViewModel => this.BindingContext as ACInputTwoViewModel;
    public ACInputTwo()
    {
      InitializeComponent();
    }
  }
}
