using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Services;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Prism.Events;

namespace ArcFlashCalculator.UnitTest.Services
{
  [TestFixture]
  class CalculatorDC : TestBase
  {
    CalculationInput input;
    CalculationOutput output;

    [SetUp]
    public void InitValues()
    {
      //set input values
      input = Container.Resolve<ICalculationInput>() as CalculationInput;
      input.MaximumShortCircuitAvailable = 5;
      input.InCabinet = false;
      input.VoltageOfBattery = 500;


      //Expected values
      output = new CalculationOutput()
      {
        IncidentEnergy = 0.0119599m,
        MinWorkinDistance = 18m,
        ArcFlashBoundary = 1.796990018m,
        IE50v = 0.001185185m,
        HazardCat = "0"
      };

      //Calc = new CalculatorService(input);
    }
    [Test]
    public void CalcIncidentEnergyDc()
    {
      Calc = Container.Resolve<ICalculatorService>() as CalculatorService;
      decimal ie = Calc.CalcIncidentEnergyDC(input, 18).Value;
      var expected = decimal.Truncate(output.IncidentEnergy * 1000) / 1000;
      var real = decimal.Truncate(ie * 1000) / 1000;
      Assert.AreEqual(expected, real);
    }

    [Test]
    public void CalculateWorkingDistanceDC()
    {
      decimal wk = Calc.CalcWorkingDistanceDC(output.IncidentEnergy, input).Value;
      var expected = decimal.Truncate(output.MinWorkinDistance * 1000) / 1000;
      var real = decimal.Truncate(wk * 1000) / 1000;
      Assert.AreEqual(expected, real);
    }

    [Test]
    public void CalculateArcFlashBoundaryDC()
    {
      decimal afb = Calc.CalcArcFlashBoundaryDC(input).Value;
      var expected = decimal.Truncate(output.ArcFlashBoundary * 1000) / 1000;
      var real = decimal.Truncate(afb * 1000) / 1000;
      Assert.AreEqual(expected, real);
    }

    [Test]
    public void CalculateIncidentEnergyL50DC()
    {
      decimal iel50 = Calc.CalcIncidentEnergyL50DC(input).Value;
      var expected = decimal.Truncate(output.IE50v * 1000) / 1000;
      var real = decimal.Truncate(iel50 * 1000) / 1000;
      Assert.AreEqual(expected, real);
    }

    [Test]
    public void CalcRiskCategoryDC()
    {
      var expected = output.HazardCat;
      var real = Calc.RiskCategoryDC(output);
      Assert.AreEqual(expected, real);
    }
  }
}
