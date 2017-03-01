
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
  public class BreakerStyle : BaseModel
  {
    public string Name { get; set; }
    public string Number { get; set; }
    public int ManufacturerId { get; set; }
    public virtual Manufacturer Manufacturer { get; set; }
  }
}
