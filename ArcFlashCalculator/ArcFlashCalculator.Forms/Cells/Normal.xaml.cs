using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.Cells
{
  public partial class Normal : ViewCell
  {
    public Normal()
    {
      InitializeComponent();
    }
    protected override void OnBindingContextChanged()
    {
      base.OnBindingContextChanged();
      if (Device.OS == TargetPlatform.iOS && string.IsNullOrEmpty(text.Text))
      {
        secondRow.Height = new GridLength(20, GridUnitType.Absolute);
      }
    }
  }
}
