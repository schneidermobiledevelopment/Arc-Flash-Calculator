using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using ArcFlashCalculator.Droid.Renderers;
using ArcFlashCalculator.Forms.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PickerRenderer = Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(Picker), typeof(ArcFlashCalculator.Droid.Renderers.PickerRenderer))]
namespace ArcFlashCalculator.Droid.Renderers
{
  public class PickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
  {
    
    protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
    {
      base.OnElementChanged(e);

      if (Control != null)
      {
        Control.Gravity = GravityFlags.Right;
        Control.Ellipsize = TextUtils.TruncateAt.End;
        Control.Background = null;
      }
    }
  }
}