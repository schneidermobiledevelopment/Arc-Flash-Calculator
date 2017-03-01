using System.Collections.Generic;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Model;

namespace ArcFlashCalculator.Core.Interfaces
{
  public interface ICalculatorService
  {
    ICalculationOutput Calculate(CalculationInput calculationInput);
    decimal? CalculateArcDuration(CalculationInput calculationInput, decimal? estimatedArcFaultCurrent);
    decimal? CalculateArcFaultCurrentAC(CalculationInput calculationInput);
    decimal? CalculateMultipleSensorRatingAC(CalculationInput calculationInput, decimal? estimatedArcFaultCurrent);
  }
}
