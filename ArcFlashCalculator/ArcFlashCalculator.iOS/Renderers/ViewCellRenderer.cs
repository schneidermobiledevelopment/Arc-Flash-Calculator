using System;
using System.Collections.Generic;
using System.Text;
using ArcFlashCalculator.Forms.CustomViews;
using ArcFlashCalculator.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ArcFlashCalculator.iOS.Renderers.ViewCellRenderer))]

  namespace ArcFlashCalculator.iOS.Renderers
  {
    class ViewCellRenderer: Xamarin.Forms.Platform.iOS.ViewCellRenderer
    {
      
      public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
      {
      var cell = base.GetCell(item, reusableCell, tv);
        
      item.Tapped += (sender, e) =>
     {
       cell.SetSelected(false,false);
     };
      //cell.SelectionStyle = UITableViewCellSelectionStyle.None;
      if (cell != null)
      {
        cell.SeparatorInset = UIEdgeInsets.Zero;
      }
      switch (item.StyleId)
      {
        case "checkmark":
          cell.Accessory = UIKit.UITableViewCellAccessory.Checkmark;
          break;
        case "detail-button":
          cell.Accessory = UIKit.UITableViewCellAccessory.DetailButton;
          break;
        case "detail-disclosure-button":
          cell.Accessory = UIKit.UITableViewCellAccessory.DetailDisclosureButton;
          break;
        case "disclosure-indicator":
          cell.Accessory = UIKit.UITableViewCellAccessory.DisclosureIndicator;
          break;
        default:
          cell.Accessory = UIKit.UITableViewCellAccessory.None;
          break;
      }
      return cell;
    }
    }
  }