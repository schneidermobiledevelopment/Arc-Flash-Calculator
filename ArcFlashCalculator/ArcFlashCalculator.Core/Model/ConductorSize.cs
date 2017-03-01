using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
  public class ConductorSize : BaseModel
  {
    public decimal OhmsPerThousandFeet { get; set; }
    public string Size { get; set; }
  }
}
