using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ArcFlashCalculator.Core.Events;
using ArcFlashCalculator.Core.Interfaces;
using Prism.Events;

namespace ArcFlashCalculator.Core.Model
{
  //Some comments sad
  public class CalculationInputSimple 
  {
    public int Id { get; set; }

    public bool? IsAlternatingCurrent { get; set; }

    public int EquipmentTypeId { get; set; }

    public bool IsSolidGround { get; set; }

    public decimal ArcDurationValue { get; set; }

    public decimal WorkingDistance { get; set; }

    public decimal NominalVoltage { get; set; }

    public decimal SourceFaultCurrent { get; set; }

    public bool IsOpenAir { get; set; }

    public bool HasCable { get; set; }

    public bool HasTransformer { get; set; }

    public int ConductorSizeId { get; set; }

    public int ConductorLength { get; set; }
    public int ConductorPerPhase { get; set; }

    public decimal? PrimaryVoltage { get; set; }
    
    public decimal? XfmrImpedance { get; set; }

    public decimal? XfmrKVA { get; set; }

    public int MaximumShortCircuitAvailable { get; set; }

    public bool InCabinet { get; set; }

    public int VoltageOfBattery { get; set; }
  }
}
