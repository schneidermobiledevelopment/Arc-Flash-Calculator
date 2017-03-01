using ArcFlashCalculator.Core.Locals;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Views
{
  public partial class Language : ContentPage
  {
    public Language()
    {
      InitializeComponent();
      IdiomsList.ItemSelected += IdiomsList_ItemSelected;
    }

    private void IdiomsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      this.Title = AppResources.Language;
    }
  }
}
