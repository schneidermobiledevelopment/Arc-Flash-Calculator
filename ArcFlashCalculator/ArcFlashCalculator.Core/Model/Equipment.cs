using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
    public class Equipment:BaseModel
    {
        public int EquipmentTypeId { get; set; }
        public decimal Voltage_Min { get; set; }
        public decimal Voltage_Max { get; set; }
        public int Typical_Bus_Gap { get; set; }
        public decimal Distance_X_Fact { get; set; }
        public int WorkingDistanceId { get; set; }
  }
}
