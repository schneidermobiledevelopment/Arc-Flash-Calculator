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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using DatePicker = Xamarin.Forms.DatePicker;


[assembly: ExportRenderer(typeof(Xamarin.Forms.DatePicker), typeof(ArcFlashCalculator.Droid.Renderers.DatePickerRenderer))]

namespace ArcFlashCalculator.Droid.Renderers
{
  class DatePickerRenderer : Xamarin.Forms.Platform.Android.DatePickerRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
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