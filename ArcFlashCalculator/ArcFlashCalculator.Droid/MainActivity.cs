using Android.App;
using Android.OS;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using Android.Content.PM;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using Xamarin.Forms;

namespace ArcFlashCalculator.Droid
{
  [Activity(Label = "Arc Flash Calculator", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {

      TabLayoutResource = Resource.Layout.tabs;
      ToolbarResource = Resource.Layout.toolbar;
//#if DEBUG
//      StrictMode.SetThreadPolicy(new StrictMode.ThreadPolicy.Builder().DetectAll().PenaltyLog().Build());
//      StrictMode.SetVmPolicy(new StrictMode.VmPolicy.Builder().DetectLeakedSqlLiteObjects().DetectLeakedClosableObjects().PenaltyLog().PenaltyDeath().Build());
//#endif
      base.OnCreate(bundle);

      global::Xamarin.Forms.Forms.Init(this, bundle);
      var cv = typeof(Xamarin.Forms.CarouselView);
      var assembly = Assembly.Load(cv.FullName);
      LoadApplication(new Forms.App(new AndroidInitializer()));

      CrashManager.Register(this);
      MetricsManager.Register(Application);

      switch (Device.Idiom)
      {
        case TargetIdiom.Phone:
          RequestedOrientation = ScreenOrientation.Portrait;
          break;
        case TargetIdiom.Tablet:
          RequestedOrientation = ScreenOrientation.User;
          break;
      }

      CheckForUpdates();
    }

    private void CheckForUpdates()
    {
      // Remove this for store builds!
      UpdateManager.Register(this);
    }

    private void UnregisterManagers()
    {
      UpdateManager.Unregister();
    }

    protected override void OnPause()
    {
      base.OnPause();
      UnregisterManagers();
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      UnregisterManagers();
    }
  }

}

