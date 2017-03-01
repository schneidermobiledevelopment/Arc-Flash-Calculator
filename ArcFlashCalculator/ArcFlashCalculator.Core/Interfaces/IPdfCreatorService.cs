using System.IO;
using ArcFlashCalculator.Core.Model;

namespace ArcFlashCalculator.Core.Interfaces
{
  public interface IPdfCreatorService
  {
    MemoryStream CreatePdfMemoryStream(CalculationInput input, CalculationOutput output);
  }
}
