using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.CustomViews
{
  public class CustomWebView : WebView
  {
   // public static readonly BindableProperty UriProperty = BindableProperty.Create<CustomWebView, string>(p=>p.Uri,default(string));
    public static readonly BindableProperty UriProperty =
      BindableProperty.Create("Uri", typeof(string), typeof(CustomWebView), null, BindingMode.TwoWay);
    public string Uri
    {
      get { return (string) GetValue(UriProperty); }
      set { SetValue(UriProperty, value);}
    }
  }
}
