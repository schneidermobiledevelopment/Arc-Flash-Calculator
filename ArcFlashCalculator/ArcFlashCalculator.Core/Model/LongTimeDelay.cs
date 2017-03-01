using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
  public class LongTimeDelay : BaseModel
  {
    public decimal? Value { get; set; }
    public int TripUnitTypeId { get; set; }
  }
}
