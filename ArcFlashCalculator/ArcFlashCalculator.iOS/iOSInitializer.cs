using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Prism;
using Microsoft.Practices.Unity;
using ArcFlashCalculator.Core.Interfaces;
using Prism.Unity;
using ArcFlashCalculator.Forms.Interfaces;
using ArcFlashCalculator.iOS.Services;

namespace ArcFlashCalculator.iOS
{
  public class iOSInitializer : IPlatformInitializer
  {
    public void RegisterTypes(IUnityContainer container)
    {
      container.RegisterType<ISaveAndLoad, SaveAndLoad>();
      container.RegisterType<ILocalize, Localize>();
    }

  }
}