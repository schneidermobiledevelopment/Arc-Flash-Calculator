using System;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Services;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.UnitTest.Values;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Prism.Events;

namespace ArcFlashCalculator.UnitTest.Services
{

  [TestFixture]
  class CalculatorAC : TestBase
  {

    [Test, Sequential]
    public void CalcBoltedFaultCurrentAC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;
      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal bfc = Calc.CalcBoltedFaultCurrentAC(input);
      var expected = RoundValue(output.BoltedFaultCurrent.Value);
      var real = RoundValue(bfc);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcArcFaulCurrentAC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);


      decimal afc = Calc.CalculateArcFaultCurrentAC(input).Value;
      var expected = RoundValue(output.EstimatedArcFaultCurrent.Value);
      var real = RoundValue(afc);
      Assert.AreEqual(expected, real);
    }

    [Test, Sequential]
    public void CalcIncidentEnergy18AC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(18m, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IE18);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcIncidentEnergy24AC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(24m, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IE24);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcIncidentEnergy36AC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(36m, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IE36);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcIncidentEnergyDistanceAC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(input.WorkingDistance.Value, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IncidentEnergy);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }

    [Test, Sequential]
    public void CalcFlashHazardBoundaryAC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      var fhb = Calc.CalcFlashHazardBoundaryAC(Calc.CalculateArcFaultCurrentAC(input));
      var expected = RoundValue(output.FPB);
      var real = RoundValue(fhb);
      Assert.AreEqual(expected, real);
    }

    [Test, Sequential]
    public void CalcRiskCategoryAC([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      var riskCat = Calc.RiskCategoryAC(output);
      var expected = output.HazardCat;
      var real = riskCat;
      Assert.AreEqual(expected, real);
    }

    //[Test, Sequential]
    //public void CalcMultipleSensor([ValueSource(typeof(CalcAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcAC), "GetCalculationOutputs")] CalculationOutput output)
    //{
    //  CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

    //  MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

    //  Calc = Container.Resolve<ICalculatorService>() as CalculatorService;
    //  var multSensor = Calc.CalculateMultipleSensorRatingAC(input, Calc.CalculateArcFaultCurrentAC(input)).Value;
    //  var expected = RoundValue(output.MultipleOfSensorRating.Value);
    //  var real = RoundValue(multSensor);
    //  Assert.AreEqual(expected, real);
    //}
  }
}