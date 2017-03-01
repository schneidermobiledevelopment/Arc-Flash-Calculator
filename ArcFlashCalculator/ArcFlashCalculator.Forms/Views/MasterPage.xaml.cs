using System;
using Prism.Navigation;
using Xamarin.Forms;
using ArcFlashCalculator.Forms.ViewModels;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class MasterPage : MasterDetailPage, IMasterDetailPageOptions
  {
    private MasterPageViewModel ViewModel => this.BindingContext as MasterPageViewModel;
    public MasterPage()
    {
      InitializeComponent();
      this.MasterBehavior = MasterBehavior.Popover;
    }
    public bool IsPresentedAfterNavigation
    {
      get
      {
        return false;
      }
    }
  }
}