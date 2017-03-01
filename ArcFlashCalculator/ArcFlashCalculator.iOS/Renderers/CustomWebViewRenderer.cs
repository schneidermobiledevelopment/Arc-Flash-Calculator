using ArcFlashCalculator.Forms.CustomViews;
using ArcFlashCalculator.iOS.Renderers;
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace ArcFlashCalculator.iOS.Renderers
{
 public class CustomWebViewRenderer : ViewRenderer<CustomWebView, UIWebView>
  {
    protected override void OnElementChanged(ElementChangedEventArgs<CustomWebView> e)
    {
      base.OnElementChanged(e);

      if (Control == null)
      {
        SetNativeControl(new UIWebView());
      }
      if (e.OldElement != null)
      {
        // Cleanup
      }
      if (e.NewElement != null)
      {
        var customWebView = Element as CustomWebView;
        var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        Directory.CreateDirectory(docsPath);
        string fileName = Path.Combine(docsPath,customWebView.Uri);
        Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
        Control.ScalesPageToFit = true;
      }
    }
  }
}