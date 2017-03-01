using System;
using ArcFlashCalculator.Core.Interfaces;
using ArcFlashCalculator.Core.Model;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Prism.Events;
namespace ArcFlashCalculator.UnitTest.Services
{
  [TestFixture]
  class CalcArcDuration : TestBase
  {


    //[Test]
    //public void CalcArc()
    //{
    //  var afc = 10000;
    //  var sensor = 2000;
    //  CalculationInput input = Container.Resolve<ICalculationInput>() as CalculationInput;
    //  input.ArcDuration = Container.Resolve<IArcDuration>() as ArcDuration;;
    //  input.ArcDuration.LongTimeDelay = new LongTimeDelay();
    //  input.ArcDuration.LongTimeDelay.Id = 0;
    //  input.ArcDuration.LongTimeDelay.Value = 0.5m;
    //  input.ArcDuration.LongTimePickup = 1;
    //  input.ArcDuration.ShortTimePickup = new ShortTimePickup();
    //  input.ArcDuration.ShortTimePickup.Id = 0;
    //  input.ArcDuration.ShortTimePickup.Value = "1.5";
    //  input.ArcDuration.ShortTimeDelay = new ShortTimeDelay();
    //  input.ArcDuration.ShortTimeDelay.Id = 0;
    //  input.ArcDuration.ShortTimeDelay.Value = "0.1 on";
    //  input.ArcDuration.Instantaneous = new Instantaneous();
    //  input.ArcDuration.Instantaneous.Id = 0;
    //  input.ArcDuration.Instantaneous.Value = "off";

    //  var y = Calc.CalculateArcDuration(input, Calc.CalculateArcFaultCurrentAC(input));
    //  Assert.AreEqual(0.729m, y);
    //}

  }
}
