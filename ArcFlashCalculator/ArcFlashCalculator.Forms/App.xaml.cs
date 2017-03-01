using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Xamarin.Forms;
using ArcFlashCalculator.Forms.Views;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Services;
using ArcFlashCalculator.Core.Model;
using Prism.Events;
using Prism.Services;
using ArcFlashCalculator.Forms.Interfaces;
using ArcFlashCalculator.Core.Locals;

namespace ArcFlashCalculator.Forms
{
  public partial class App : PrismApplication
  {
    private readonly ILocalize _localize;
    public App(IPlatformInitializer initializer = null) : base(initializer) {
      var localize = Container.Resolve<ILocalize>() ;
      _localize = localize;
      if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
      {
        LanguageSettings.CurrentCulture = _localize.GetCurrentCultureInfo();
        AppResources.Culture = LanguageSettings.CurrentCulture; // set the RESX for resource localization
        _localize.SetLocale(LanguageSettings.CurrentCulture); // set the Thread for locale-aware methods
      }
    }

    protected override async void OnInitialized()
    {
      InitializeComponent();
      //await NavigationService.NavigateAsync("MasterDetail/NavigationPage/CommonInput");
      await NavigationService.NavigateAsync("MasterPage/NavPage/CommonInput");
      //await NavigationService.NavigateAsync("NavPage/CalculateArcDurationOne");
    }

    protected override void RegisterTypes()
    {

      //Mobile

      Container.RegisterType<IEventAggregator, EventAggregator>();
      Container.RegisterType<IPageDialogService, PageDialogService>();
      //Common
      Container.RegisterType<IDataService, DataService>();
      Container.RegisterType<IValidator<CalculationInput>, CalculationInputValidator>();
      Container.RegisterType<ICalculationInput, CalculationInput>();
      Container.RegisterType<IArcDuration, ArcDuration>();
      Container.RegisterType<IValidator<ArcDuration>, ArcDurationValidator>();
      Container.RegisterType<ICalculationOutput, CalculationOutput>();
      Container.RegisterType<ICalculatorService, CalculatorService>();
      Container.RegisterType<IPdfCreatorService, PdfCreatorService>();
      Container.RegisterType<ILocalize>();

      //Pages
      Container.RegisterTypeForNavigation<NavigationPage>();
      Container.RegisterTypeForNavigation<MasterDetail>();
      Container.RegisterTypeForNavigation<LeftMenu>();
      Container.RegisterTypeForNavigation<Settings>();
      Container.RegisterTypeForNavigation<About>();
      Container.RegisterTypeForNavigation<PdfView>();
      Container.RegisterTypeForNavigation<CommonInput>();
      Container.RegisterTypeForNavigation<ACInputOne>();
      Container.RegisterTypeForNavigation<ACInputTwo>();
      Container.RegisterTypeForNavigation<ACHasCable>();
      Container.RegisterTypeForNavigation<ACHasXfmr>();
      Container.RegisterTypeForNavigation<ACSensor>();
      Container.RegisterTypeForNavigation<ACCircuitDiagram>();
      Container.RegisterTypeForNavigation<ParameterSummary>();
      Container.RegisterTypeForNavigation<DCInput>();
      Container.RegisterTypeForNavigation<DCCircuitDiagram>();
      //Container.RegisterTypeForNavigation<ACCableXfmr>();
      Container.RegisterTypeForNavigation<ACCableAndXfmr>();
      Container.RegisterTypeForNavigation<StartOverPopup>();
      Container.RegisterTypeForNavigation<TechnicalHelp>();
      Container.RegisterTypeForNavigation<TripCurves>();
      Container.RegisterTypeForNavigation<OpenExternalPopup>();
      Container.RegisterTypeForNavigation<ACResult>();
      Container.RegisterTypeForNavigation<FindSafeWorkingDistance>();
      Container.RegisterTypeForNavigation<WorkingDistanceTable>();
      Container.RegisterTypeForNavigation<ProtectingClothing>();
      Container.RegisterTypeForNavigation<CalculateArcDurationOne>();
      Container.RegisterTypeForNavigation<CalculateArcDurationTwo>();
      Container.RegisterTypeForNavigation<NavPage>();
      Container.RegisterTypeForNavigation<MasterPage>();
      Container.RegisterTypeForNavigation<Language>();
      Container.RegisterTypeForNavigation<Language>();
      Container.RegisterTypeForNavigation<DateFormat>();
      Container.RegisterTypeForNavigation<MeasuresUnits>();
    }
  }
}