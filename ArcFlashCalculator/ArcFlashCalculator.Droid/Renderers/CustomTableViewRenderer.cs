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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;
using View = Android.Views.View;
using Android.Util;

namespace ArcFlashCalculator.Droid.Renderers
{
  public class CustomTableViewRenderer : TableViewModelRenderer
  {
    public CustomTableViewRenderer(Context context, ListView listView, TableView view)
        : base(context, listView, view)
    {
    }
    public override View GetView(int position, View convertView, ViewGroup parent)
    {
      var view = base.GetView(position, convertView, parent);
      if (GetCellForPosition(position).GetType() != typeof(TextCell)) return view;
      var layout = (LinearLayout)view;
      var text = (TextView)((LinearLayout)((LinearLayout)layout.GetChildAt(0)).GetChildAt(1)).GetChildAt(0);
      text.SetTextColor(Color.Accent.ToAndroid());
      text.SetTextSize(ComplexUnitType.Dip, 16);
      text.Gravity = GravityFlags.CenterHorizontal;
      
      //text.SetBackgroundColor(Color.FromHex(Resource.Color.).ToAndroid());
      var divider = layout.GetChildAt(0);

      divider.SetBackgroundColor(Color.FromHex("#E6E6E6").ToAndroid());
      if (string.IsNullOrEmpty(text.Text))
      {
        divider.Visibility=ViewStates.Gone;
      }
      return view;
    }
  }
}