using System;
using System.Collections.Generic;
using System.ComponentModel;
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

[assembly: ExportRenderer(typeof(Entry), typeof(EntryNoBorder))]
namespace ArcFlashCalculator.iOS.Renderers
{
  class EntryNoBorder : EntryRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
      base.OnElementChanged(e);
      Control.BorderStyle = UITextBorderStyle.None;
      var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f));
      if (Device.Idiom == TargetIdiom.Phone)
      {
        toolbar.Items = new[]
        {
          new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
          new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
        };

        this.Control.InputAccessoryView = toolbar;
      }
    }
  }
}
    