using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace ArcFlashCalculator.UnitTest
{
  [TestFixture]
  public class RealisticInputOutputTests : TestBase
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
    public void HasCableNoTransformerTest()
    {
      input.IsAlternatingCurrent = true;
      input.EquipmentTypeId = 0; //open air
      input.IsSolidGround = true;
      input.NominalVoltage = 4200;
      input.SourceFaultCurrent = 15349;
      input.IsOpenAir = false;
      input.HasTransformer = false;
      input.HasCable = true;
      input.ConductorSizeId = 9; //250
      input.ConductorPerPhase = 3;
      input.ConductorLength = 54;
      input.ArcDurationValue = 1.81m;

      output = Calc.Calculate(input) as CalculationOutput;

      Assert.AreEqual(output.IncidentEnergy, 20.39m);
    }
  }
}
