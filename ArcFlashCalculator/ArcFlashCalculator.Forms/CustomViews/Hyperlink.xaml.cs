using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.CustomViews
{
  public partial class Hyperlink : ContentView
  {
    public Hyperlink()
    {
      InitializeComponent();
    }

    public string Text {
      set
      {
        text.Text= value;
      }
    }
  }
}
