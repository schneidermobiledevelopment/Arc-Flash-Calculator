using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcFlashCalculator.Forms.CustomViews;
using ArcFlashCalculator.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ArcFlashCalculator.iOS.Renderers
{
  class CustomTableViewRenderer: TableViewModelRenderer
  {
    private TableViewModelRenderer _renderer;
    public CustomTableViewRenderer(TableView table, TableViewModelRenderer renderer)
         : base(table)
    {
      _renderer = renderer;
    }
    public override UIView GetViewForHeader(UITableView tableView, nint section)
    { 

      tableView.BackgroundColor=UIColor.White;
      tableView.TintColor = UIColor.FromRGB(51, 145, 82);
      return base.GetViewForHeader(tableView, section);
    }

    public override nfloat GetHeightForHeader(UITableView tableView, nint section)
    {
      
      if (tableView.NumberOfSections() != null)
      {
       var title =TitleForHeader(tableView, section);
        if (string.IsNullOrEmpty(title))
          return 0.02f;
      }
      //switch (Device.Idiom)
      //{
      //case    TargetIdiom.Phone:
      //    return 45;
      //  case TargetIdiom.Tablet:
      //    return 55;
      //}
      return 45;
    }

    public override nfloat GetHeightForFooter(UITableView tableView, nint section)
    {
      //return base.GetHeightForFooter(tableView, section);
      return 2f;
    }

    public override string TitleForHeader(UITableView tableView, nint section)
    {
      return base.TitleForHeader(tableView, section);
    }


    public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
    {
      if (headerView != null)
      {
        var header = headerView as UITableViewHeaderFooterView;
        header.TextLabel.TextColor = UIColor.FromRGB(51, 145, 82);
        header.TextLabel.TextAlignment = UITextAlignment.Center;
        switch (Device.Idiom)
        {
          case TargetIdiom.Phone:
            header.TextLabel.Font = UIFont.BoldSystemFontOfSize(14);
            break;
          case TargetIdiom.Tablet:
            header.TextLabel.Font = UIFont.BoldSystemFontOfSize(18);
            break;

        }
        
        
        header.BackgroundView.BackgroundColor = UIColor.FromRGB(230, 230, 230);
      }
    }
  }
}