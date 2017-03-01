
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
  public class Manufacturer : BaseModel
  {
    public string Name { get; set; }
    public virtual ICollection<BreakerStyle> DeviceModels { get; set; }
  }
}
