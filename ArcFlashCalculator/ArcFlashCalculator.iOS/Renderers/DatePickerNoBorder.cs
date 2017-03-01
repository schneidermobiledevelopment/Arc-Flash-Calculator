using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcFlashCalculator.Forms.CustomViews;
using ArcFlashCalculator.iOS.Renderers;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerNoBorder))]
namespace ArcFlashCalculator.iOS.Renderers
{
  class DatePickerNoBorder : DatePickerRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
    {
      base.OnElementChanged(e);
      Control.BorderStyle = UITextBorderStyle.None;
      Control.TextAlignment=UITextAlignment.Right;
    }
  }
}