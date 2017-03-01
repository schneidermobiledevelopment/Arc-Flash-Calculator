using ArcFlashCalculator.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Interfaces
{
  public interface IShowingAndHidingService
  {
    List<VisibilityModel> ShowingAndHidingService(CalculationInput calculationInput);
  }
}
