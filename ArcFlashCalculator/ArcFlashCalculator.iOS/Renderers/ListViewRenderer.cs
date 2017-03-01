using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ListViewRenderer = ArcFlashCalculator.iOS.Renderers.ListViewRenderer;

[assembly: ExportRenderer(typeof(ListView), typeof(ListViewRenderer))]
namespace ArcFlashCalculator.iOS.Renderers
{
  class ListViewRenderer:Xamarin.Forms.Platform.iOS.ListViewRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
    {
      base.OnElementChanged(e);
      if (Control != null)
      {
        Control.SeparatorInset = UIEdgeInsets.Zero;
        var tableView = Control as UITableView;
        this.Control.TableFooterView = new UIView();
        if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
          tableView.CellLayoutMarginsFollowReadableWidth = false;
      }
      return;
    }
  }
}
