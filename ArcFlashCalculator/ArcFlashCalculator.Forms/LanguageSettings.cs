using ArcFlashCalculator.Core.Locals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Forms
{
  public static class LanguageSettings
  {
    public static CultureInfo CurrentCulture { get; set; }
    //public static CultureInfo CurrentCulture;
    public static CultureInfo CurrentUiCulture;

    public static void ChangeCurrentCultureInfo(CultureInfo cultureInfo)
    {
      CurrentCulture = cultureInfo;
      CurrentUiCulture = cultureInfo;
      AppResources.Culture = cultureInfo;
    }
  }
}
