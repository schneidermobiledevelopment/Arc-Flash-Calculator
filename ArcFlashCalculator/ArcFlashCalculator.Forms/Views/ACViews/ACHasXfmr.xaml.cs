using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class ACHasXfmr : ContentPage
  {
    private ACHasXfmrViewModel ViewModel => this.BindingContext as ACHasXfmrViewModel;
    public ACHasXfmr()
    {
      InitializeComponent();
    }


    protected override void OnAppearing()
    {
      base.OnAppearing();
      if (ViewModel != null)
      {
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
