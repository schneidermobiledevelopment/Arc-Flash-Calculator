using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
  public class TripUnitType : BaseModel
  {
    public string Name { get; set; }
    public int BreakerStyleId { get; set; }
    public virtual BreakerStyle BreakerStyle { get; set; }
  }
}
