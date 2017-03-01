using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
  public class EquipmentType : BaseModel
  {
    public string Name { get; set; }
    public bool IsLowVoltageOnly { get; set; }
  }
}
