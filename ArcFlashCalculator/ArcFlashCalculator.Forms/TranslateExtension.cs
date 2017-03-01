using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using ArcFlashCalculator.Core.Locals;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ArcFlashCalculator.Forms
{
  [ContentProperty("Text")]
  public class TranslateExtension : IMarkupExtension
  {
    public CultureInfo ci;
    const string ResourceId = "ArcFlashCalculator.Core.Locals.AppResources";
    
    public TranslateExtension()
    {
      //ci = CultureInfo.CurrentCulture;
      //LanguageSettings.CurrentCulture = CultureInfo.CurrentCulture;
      //if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
      //{
      //  ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
      //}
    }


    public string Text { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
      if (Text == null)
        return "";
      ResourceManager resmgr = new ResourceManager(ResourceId, typeof(AppResources).GetTypeInfo().Assembly);
      var translation = resmgr.GetString(Text, LanguageSettings.CurrentCulture);

      if (translation == null)
      {
#if DEBUG
        throw new ArgumentException(
            String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, LanguageSettings.CurrentCulture.Name),
            "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
      }
      return translation;
    }
  }
}