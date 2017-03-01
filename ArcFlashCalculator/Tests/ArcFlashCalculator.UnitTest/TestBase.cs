using System;
using System.IO;
using System.Linq;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.Core.Services;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Prism.Events;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ArcFlashCalculator.UnitTest
{
    public class TestBase
    {
    public UnityContainer Container;
    public CalculatorService Calc;

    [SetUp]
    public void Initialize()
    {
      // register ServiceA and ViewModel
      Container = new UnityContainer();
      Container.RegisterType<IEventAggregator, EventAggregator>();
      Container.RegisterType<IDataService, DataService>();
      Container.RegisterType<IValidator<CalculationInput>, CalculationInputValidator>();
      Container.RegisterType<ICalculationInput, CalculationInput>();
      Container.RegisterType<IArcDuration, ArcDuration>();
      Container.RegisterType<IValidator<ArcDuration>, ArcDurationValidator>();
      Container.RegisterType<ICalculationOutput, CalculationOutput>();
      Container.RegisterType<ICalculatorService, CalculatorService>();
      Container.RegisterType<IShowingAndHidingService, ShowingAndHidingSevice>();

      Calc = Container.Resolve<ICalculatorService>() as CalculatorService;

    }

    public void MapCalculationInputSimpleToCalculationInput(ref CalculationInput input, ref CalculationInputSimple inputSimple)
    {
      input.Id = inputSimple.Id;
      input.IsAlternatingCurrent = inputSimple.IsAlternatingCurrent;
      input.EquipmentTypeId = inputSimple.EquipmentTypeId;
      input.ArcDurationValue = inputSimple.ArcDurationValue;
      input.WorkingDistance = inputSimple.WorkingDistance;
      input.NominalVoltage = inputSimple.NominalVoltage;
      input.SourceFaultCurrent = inputSimple.SourceFaultCurrent;
      input.IsSolidGround = inputSimple.IsSolidGround;
      input.IsOpenAir = inputSimple.IsOpenAir;
      input.HasCable = inputSimple.HasCable;
      input.HasTransformer = inputSimple.HasTransformer;
      input.ConductorSizeId = inputSimple.ConductorSizeId;
      input.ConductorPerPhase = inputSimple.ConductorPerPhase;
      input.ConductorLength = inputSimple.ConductorLength;
      input.PrimaryVoltage = inputSimple.PrimaryVoltage;
      input.XfmrImpedance = inputSimple.XfmrImpedance;
      input.XfmrKVA = inputSimple.XfmrKVA;
      input.MaximumShortCircuitAvailable = inputSimple.MaximumShortCircuitAvailable;
      input.InCabinet = inputSimple.InCabinet;
      input.VoltageOfBattery = inputSimple.VoltageOfBattery;
    }

    public decimal RoundValue(decimal value)
    {
      return Math.Truncate(value * 1000000) / 1000000;
    }
  }
}

