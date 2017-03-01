using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcFlashCalculator.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(Picker), typeof(PickerNoBorder))]
namespace ArcFlashCalculator.iOS.Renderers
{
  class PickerNoBorder : PickerRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
    {
      base.OnElementChanged(e);
      Control.BorderStyle = UITextBorderStyle.None;
      Control.TextAlignment = UITextAlignment.Right;
    }
  }
}