using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ArcFlashCalculator.WPF.Views.Modals
{
  /// <summary>
  /// Interaction logic for FindEquipmentTripCurvesView.xaml
  /// </summary>
  public partial class FindEquipmentTripCurvesView : UserControl
  {
    public FindEquipmentTripCurvesView()
    {
      InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }
  }
}
