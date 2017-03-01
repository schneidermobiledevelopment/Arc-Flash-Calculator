using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using ArcFlashCalculator.UnitTest.Values;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace ArcFlashCalculator.UnitTest.Services
{
  [TestFixture]
  class CalculatorNoTransformerAc : TestBase
  {
    [Test, Sequential]
    public void CalcBoltedFaultCurrentAC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;
      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal bfc = Calc.CalcBoltedFaultCurrentAC(input);
      var expected = RoundValue(output.BoltedFaultCurrent.Value);
      var real = RoundValue(bfc);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcArcFaulCurrentAC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);


      decimal afc = Calc.CalculateArcFaultCurrentAC(input).Value;
      var expected = RoundValue(output.EstimatedArcFaultCurrent.Value);
      var real = RoundValue(afc);
      Assert.AreEqual(expected, real);
    }

    [Test, Sequential]
    public void CalcIncidentEnergy18AC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(18m, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IE18);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcIncidentEnergy24AC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(24m, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IE24);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcIncidentEnergy36AC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(36m, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IE36);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }
    [Test, Sequential]
    public void CalcIncidentEnergyDistanceAC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      decimal IE = Calc.CalcIncidentEnergyAC(input.WorkingDistance.Value, Calc.CalculateArcFaultCurrentAC(input)).Value;
      var expected = RoundValue(output.IncidentEnergy);
      var real = RoundValue(IE);
      Assert.AreEqual(expected, real);
    }

    [Test, Sequential]
    public void CalcFlashHazardBoundaryAC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      var fhb = Calc.CalcFlashHazardBoundaryAC(Calc.CalculateArcFaultCurrentAC(input));
      var expected = RoundValue(output.FPB);
      var real = RoundValue(fhb);
      Assert.AreEqual(expected, real);
    }

    [Test, Sequential]
    public void CalcRiskCategoryAC([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    {
      CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;

      MapCalculationInputSimpleToCalculationInput(ref input, ref inputSimple);

      var riskCat = Calc.RiskCategoryAC(output);
      var expected = output.HazardCat;
      var real = riskCat;
      Assert.AreEqual(expected, real);
    }

    //[Test, Sequential]
    //public void CalcMultipleSensor([ValueSource(typeof(CalcNoXAC), "GetCalculationInputSimples")] CalculationInputSimple inputSimple, [ValueSource(typeof(CalcNoXAC), "GetCalculationOutputs")] CalculationOutput output)
    //{
    //  Calc = new CalculatorService(input);
    //  var multSensor = Calc.CalcMultipleSensorRatingAC(output);
    //  var expected = RoundValue(output.MultipleOfSensorRating);
    //  var real = RoundValue(multSensor);
    //  Assert.AreEqual(expected, real);
    //}
  }
}