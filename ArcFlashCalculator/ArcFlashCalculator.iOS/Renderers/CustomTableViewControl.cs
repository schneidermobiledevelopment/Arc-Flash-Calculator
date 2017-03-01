using System;
using System.Collections.Generic;
using System.Text;
using ArcFlashCalculator.Forms.CustomViews;
using ArcFlashCalculator.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(CustomTableView), typeof(CustomTableViewControl))]
namespace ArcFlashCalculator.iOS.Renderers
{
  class CustomTableViewControl: TableViewRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
    {
      base.OnElementChanged(e);

      if (e.OldElement == null)
      {
        var tableView = Control;
        tableView.Source = new CustomTableViewRenderer(Element, (TableViewModelRenderer)tableView.Source);
      }
    }
  }
}
