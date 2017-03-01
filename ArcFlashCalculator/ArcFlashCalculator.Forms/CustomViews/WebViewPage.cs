using Xamarin.Forms;

namespace ArcFlashCalculator.Forms.CustomViews
{
  public class WebViewPage : ContentPage
  {
    public WebViewPage()
    {
      Padding = new Thickness(0,20,0,0);
      Content = new StackLayout
      {
        Children = {
          new CustomWebView()
          {
            Uri = "ArcFlashEstimateReport.pdf",
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand
          }
        }
      };
    }
  }
}
