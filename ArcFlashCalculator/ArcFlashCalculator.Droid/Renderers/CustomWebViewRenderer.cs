using System.Net;
using ArcFlashCalculator.Droid.Renderers;
using ArcFlashCalculator.Forms.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace ArcFlashCalculator.Droid.Renderers
{
  public class CustomWebViewRenderer : WebViewRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
    {
      base.OnElementChanged(e);

      if (e.NewElement != null)
      {
        var customWebView = Element as CustomWebView;
        Control.Settings.AllowUniversalAccessFromFileURLs = true;
        Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///data/user/0/SchneiderElectric.ArcFlashCalculator.Droid/files/{0}", WebUtility.UrlEncode(customWebView.Uri))));
      }
    }
  }
}
