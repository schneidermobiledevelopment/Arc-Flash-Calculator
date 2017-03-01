using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(ArcFlashCalculator.Droid.Renderers.EntryRenderer))]
namespace ArcFlashCalculator.Droid.Renderers
{
  class EntryRenderer:Xamarin.Forms.Platform.Android.EntryRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
      base.OnElementChanged(e);
      Control.Background = null;
    }
  }
}