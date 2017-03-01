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
using ListView = Android.Widget.ListView;
using ArcFlashCalculator.Forms.CustomViews;
using ArcFlashCalculator.Droid.Renderers;

[assembly: ExportRenderer(typeof(CustomTableView), typeof(CustomTableViewControl ))]
namespace ArcFlashCalculator.Droid.Renderers
{
  public class CustomTableViewControl : TableViewRenderer
  {
    protected override TableViewModelRenderer GetModelRenderer(ListView listView, TableView view)
    {
      return new CustomTableViewRenderer(Context, listView, view);
    }
  }
}