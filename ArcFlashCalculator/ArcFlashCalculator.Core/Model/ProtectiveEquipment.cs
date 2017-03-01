using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcFlashCalculator.Core.Model
{
    public class ProtectiveEquipment :BaseModel
    {
        public string RiskCategory { get; set; }
        public decimal Rating_Min { get; set; }
        public decimal Rating_Max { get; set; }
    }
}
