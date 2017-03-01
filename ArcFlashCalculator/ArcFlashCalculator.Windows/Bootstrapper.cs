using System.Windows;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Services;
using ArcFlashCalculator.WPF.Controls;
using ArcFlashCalculator.WPF.Views;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace ArcFlashCalculator.WPF
{
  public class Bootstrapper : UnityBootstrapper
  {
    protected override DependencyObject CreateShell()
    {
      return Container.Resolve<MainWindowView>();
    }

    protected override void InitializeShell()
    {
      Application.Current.MainWindow.Show();
    }

    protected override void ConfigureContainer()
    {
      base.ConfigureContainer();
      Container.RegisterTypeForNavigation<MainWindowView>("MainWindowView");
      Container.RegisterTypeForNavigation<FormView>("FormView");
      Container.RegisterTypeForNavigation<ConfirmationView>("ConfirmationView");
      Container.RegisterTypeForNavigation<ResultsView>("ResultsView");
      Container.RegisterTypeForNavigation<DiagramView>("DiagramView");
      Container.RegisterTypeForNavigation<DiagramView>("ArcDurationCalculationView");

      Container.RegisterType<IDataService, DataService>();
      Container.RegisterType<IValidator<CalculationInput>, CalculationInputValidator>();
      Container.RegisterType<ICalculationInput, CalculationInput>();
      Container.RegisterType<IArcDuration, ArcDuration>();
      Container.RegisterType<IValidator<ArcDuration>, ArcDurationValidator>();
      Container.RegisterType<ICalculationOutput, CalculationOutput>();
      Container.RegisterType<ICalculatorService, CalculatorService>();
      Container.RegisterType<IShowingAndHidingService, ShowingAndHidingSevice>();
      Container.RegisterType<IPdfCreatorService, PdfCreatorService>();
      Container.RegisterType<ISaveAndLoad, SaveAndLoad>();
    }
  }
}
