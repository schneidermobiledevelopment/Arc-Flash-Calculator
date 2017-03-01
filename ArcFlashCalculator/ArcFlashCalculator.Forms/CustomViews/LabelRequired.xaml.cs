using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.CustomViews
{
  public partial class LabelRequired : ContentView
  {
    public string Text {
      set {
        lbl.Text=value ;
      }
      get { return lbl.Text; }
    }

    public bool IsRequired {
      set {start.IsVisible=value; } }

    public LabelRequired() 
    {
      InitializeComponent();
    }
  }
}
