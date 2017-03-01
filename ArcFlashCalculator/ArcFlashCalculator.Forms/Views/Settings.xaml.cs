using ArcFlashCalculator.Core.Locals;
using ArcFlashCalculator.Forms.Models;
using ArcFlashCalculator.Forms.ViewModels;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class Settings : ContentPage
  {
    private SettingsViewModel ViewModel => this.BindingContext as SettingsViewModel;
    public Settings()
    {
      InitializeComponent();
      settingsList.ItemTapped += SettingsList_ItemTapped;
    }

    private void SettingsList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
      ViewModel.OnSelectedChangedCommand.Execute((SettingItem)e.Item);
    }
    protected override void OnAppearing()
    {
      base.OnAppearing();
      this.Title = AppResources.Settings;
      ViewModel.InitList();
    }

    protected override bool OnBackButtonPressed()
    {
      return true;
    }
  }
}
