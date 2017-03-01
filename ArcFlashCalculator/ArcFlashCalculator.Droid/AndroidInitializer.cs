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
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Forms.Interfaces;
using ArcFlashCalculator.Droid.Services;

namespace ArcFlashCalculator.Droid
{
  public class AndroidInitializer : IPlatformInitializer
  {
    public void RegisterTypes(IUnityContainer container)
    {
      container.RegisterType<ISaveAndLoad, SaveAndLoad>();
      container.RegisterType<ILocalize, Localize>();
    }
  }
}