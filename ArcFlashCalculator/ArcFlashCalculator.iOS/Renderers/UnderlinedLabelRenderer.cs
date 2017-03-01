using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcFlashCalculator.Forms.CustomViews;
using ArcFlashCalculator.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(UnderlinedLabel), typeof(UnderlinedLabelRenderer))]
namespace ArcFlashCalculator.iOS.Renderers
{
  class UnderlinedLabelRenderer:LabelRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
    {
      base.OnElementChanged(e);
      try
      {
        var label = (UILabel)Control;
        var text = (NSMutableAttributedString)label.AttributedText;
        var range = new NSRange(0, text.Length);
        text.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Cannot underline Label. Error: ", ex.Message);
      }
    }
  }
}