using ArcFlashCalculator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcFlashCalculator.Core.Model;

namespace ArcFlashCalculator.Core.Services
{
  public class ShowingAndHidingSevice : IShowingAndHidingService
  {
    public List<VisibilityModel> ShowingAndHidingService(CalculationInput calculationInput)
    {

      CalculationInput CalculationInput = calculationInput;
      bool ShowAlternatingCurrentFields, ShowDirectCurrentFields, ShowConductorFields, ShowTransformerFields;
      if (!CalculationInput.IsAlternatingCurrent.HasValue)
      {
        ShowAlternatingCurrentFields = CalculationInput.IsAlternatingCurrent ?? false;
        ShowDirectCurrentFields = !CalculationInput.IsAlternatingCurrent ?? false;
        ShowConductorFields = CalculationInput.HasCable ?? false;
        ShowTransformerFields = CalculationInput.HasTransformer ?? false;
      }
      else
      {
        //HasErrors = calculationInput.ErrorCount > 0;
        //ErrorCount = calculationInput.ErrorCount;
        //IsDirectCurrent = !CalculationInput.IsAlternatingCurrent ?? false;
        ShowConductorFields = false;
        ShowTransformerFields = false;
        ShowAlternatingCurrentFields = calculationInput.IsAlternatingCurrent ?? false;
        ShowDirectCurrentFields = !calculationInput.IsAlternatingCurrent ?? false;
        if (ShowAlternatingCurrentFields)
        {
          ShowConductorFields = calculationInput.HasCable ?? false;
          ShowTransformerFields = calculationInput.HasTransformer ?? false;
        }

       // CalculationInput.HasTransformer = CalculationInput.IsAlternatingCurrent.HasValue && !CalculationInput.IsAlternatingCurrent.Value ? null : CalculationInput.HasTransformer;
      }

      List<VisibilityModel> Fields = new List<VisibilityModel>();
      Fields.Add(new VisibilityModel() { Parameter = "ShowAlternatingCurrentFields", ParameterValue = ShowAlternatingCurrentFields });
      Fields.Add(new VisibilityModel() { Parameter = "ShowDirectCurrentFields", ParameterValue = ShowDirectCurrentFields });
      Fields.Add(new VisibilityModel() { Parameter = "ShowConductorFields", ParameterValue = ShowConductorFields });
      Fields.Add(new VisibilityModel() { Parameter = "ShowTransformerFields", ParameterValue = ShowTransformerFields });

      return Fields;
//      throw new NotImplementedException();
    }
  }
}
