using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ArcFlashCalculator.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ArcFlashCalculator.Forms.CustomViews;

[assembly: ExportRenderer(typeof(UnderlinedLabel), typeof(UnderlinedLabelRenderer))]
namespace ArcFlashCalculator.Droid.Renderers
{
  public class UnderlinedLabelRenderer : LabelRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
    {
      base.OnElementChanged(e);
      try
      {
        var textView = (TextView)Control;
        textView.PaintFlags |= PaintFlags.UnderlineText;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Cannot underline Label. Error: ", ex.Message);
      }
    }
  }
}