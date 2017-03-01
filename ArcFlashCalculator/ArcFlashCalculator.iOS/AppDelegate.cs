
using System;
using Foundation;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;
using UIKit;
using System.Reflection;
using HockeyApp.iOS;
using SegmentedControl.FormsPlugin.iOS;

namespace ArcFlashCalculator.iOS
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register("AppDelegate")]
  public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
  {
    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      global::Xamarin.Forms.Forms.Init();
      var cv = typeof(Xamarin.Forms.CarouselView);
      var assembly = Assembly.Load(cv.FullName);
      SegmentedControlRenderer.Init();
      LoadApplication(new Forms.App(new iOSInitializer()));


      //var cv = typeof(Xamarin.Forms.CarouselView);
      //var assembly = Assembly.Load(cv.FullName);

      app.StatusBarStyle = UIStatusBarStyle.LightContent;
      //UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(61, 205, 88);
      //UINavigationBar.Appearance.TintColor = UIColor.White;
      //UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
      //{
      //  TextColor = UIColor.White
      //});
      var manager = BITHockeyManager.SharedHockeyManager;
      manager.Configure("7ac05bace8c64acc9016fe633835f58c");
      manager.DisableMetricsManager = true;
      manager.StartManager();
      manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds



      return base.FinishedLaunching(app, options);
    }
  }


 

}
