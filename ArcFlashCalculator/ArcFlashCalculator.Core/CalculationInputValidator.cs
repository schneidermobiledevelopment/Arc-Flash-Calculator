using FluentValidation;

namespace ArcFlashCalculator.Core.Model
{
  public class CalculationInputValidator : FluentValidator<CalculationInput>
  {
    public CalculationInputValidator()
    {
      CascadeMode = CascadeMode.StopOnFirstFailure;

      RuleFor(o => o.IsAlternatingCurrent)
        .NotEmpty()
        .WithMessage("Required Field"); //these are never used 
      //TODO Change to Constant

      RuleFor(o => o.EquipmentType)
        .Must(
          (input, equipmentType) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return equipmentType != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.IsSolidGround)
        .Must(
          (input, isSolidGround) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return isSolidGround != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.IsOpenAir)
        .Must(
          (input, isOpenAir) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return isOpenAir != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.HasCable)
        .Must(
          (input, hasCable) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return hasCable != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.HasTransformer)
        .Must(
          (input, hasTransformer) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return hasTransformer != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.NominalVoltage)
        .Must(
          (input, nominalVoltage) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return nominalVoltage != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, nominalVoltage) =>
            {
              // We validate this rule only when there IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value && input.EquipmentType != null && (input.EquipmentType.Id == 2 || input.EquipmentType.Id == 4))
              {
                return input.NominalVoltageMin <= nominalVoltage && nominalVoltage <= input.NominalVoltageLowVoltageMax;
              }

              return true;
            })
        .WithMessage("Expected between 30V and 1000V")
        .Must(
            (input, nominalVoltage) =>
            {
              // We validate this rule only when there IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value && input.EquipmentType != null && (input.EquipmentType.Id == 3 || input.EquipmentType.Id == 5))
              {
                return input.NominalVoltageMediumVoltageMin <= nominalVoltage && nominalVoltage <= input.NominalVoltageMax;
              }

              return true;
            })
        .WithMessage("Expected between 1001V and 36000V")
        .Must(
              (input, nominalVoltage) =>
              {
                // We validate this rule only when there IsAlternatingCurrent
                if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
                {
                  return input.NominalVoltageMin <= nominalVoltage && nominalVoltage <= input.NominalVoltageMax;
                }

                // If IsDirectCurrent, we shouldn't show the error message.
                return true;
              }
              )
          .WithMessage("Expected between 30V and 36000V");

      RuleFor(o => o.SourceFaultCurrent)
        .Must(
          (input, sourceFaultCurrent) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return sourceFaultCurrent != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, sourceFaultCurrent) =>
            {
              // We validate this rule only when there IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
              {
                return input.SourceFaultCurrentMin <= sourceFaultCurrent && sourceFaultCurrent <= input.SourceFaultCurrentMax;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 1A and 99999A");

      RuleFor(o => o.ConductorSize)
        .Must(
          (input, conductorSize) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasCable.HasValue && input.HasCable.Value)
            {
              return conductorSize != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.ConductorPerPhase)
        .Must(
          (input, conductorPerPhase) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasCable.HasValue && input.HasCable.Value)
            {
              return conductorPerPhase != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.ConductorLength)
        .Must(
          (input, conductorLength) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasCable.HasValue && input.HasCable.Value)
            {
              return conductorLength != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.SensorRating)
        .Must(
            (input, sensorRating) =>
            {
              // We validate this rule only when there IsAlternatingCurrent and there is a sensor rating value
              if (sensorRating.HasValue && input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
              {
                return input.SensorRatingMin <= sensorRating && sensorRating <= input.SensorRatingMax;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 15 and 6000");

      RuleFor(o => o.PrimaryVoltage)
        .Must(
          (input, primaryVoltage) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasTransformer.HasValue && input.HasTransformer.Value)
            {
              return primaryVoltage != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, primaryVoltage) =>
            {
              // We validate this rule only when there IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasTransformer.HasValue && input.HasTransformer.Value)
              {
                return input.PrimaryVoltageMin <= primaryVoltage && primaryVoltage <= input.PrimaryVoltageMax;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 208V and 138000V");

      RuleFor(o => o.XfmrImpedance)
        .Must(
          (input, xfmrImpedance) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasTransformer.HasValue && input.HasTransformer.Value)
            {
              return xfmrImpedance != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, xfmrImpedance) =>
            {
              // We validate this rule only when there IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasTransformer.HasValue && input.HasTransformer.Value)
              {
                return input.XfmrImpedanceMin <= xfmrImpedance && xfmrImpedance <= input.XfmrImpedanceMax;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 1 and 15");

      RuleFor(o => o.XfmrKVA)
        .Must(
          (input, xfmrKVA) =>
          {
            // We validate this rule only when there is IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasTransformer.HasValue && input.HasTransformer.Value)
            {
              return xfmrKVA != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, xfmrKVA) =>
            {
              // We validate this rule only when there IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value
                && input.HasTransformer.HasValue && input.HasTransformer.Value)
              {
                return xfmrKVA > input.XfmrKVAMin;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 208V and 36000V");


      //DC Rules
      RuleFor(o => o.MaximumShortCircuitAvailable)
        .Must(
          (input, maxShortAvailable) =>
          {
            // We validate this rule only when there is !IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && !input.IsAlternatingCurrent.Value)
            {
              return maxShortAvailable != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, maxShortAvailable) =>
            {
              // We validate this rule only when there !IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && !input.IsAlternatingCurrent.Value)
              {
                return input.MaximumAvailableShortCircuitMin <= maxShortAvailable && maxShortAvailable <= input.MaximumAvailableShortCircuitMax;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 1000V and 50000V");

      RuleFor(o => o.InCabinet)
        .Must(
          (input, inCabinet) =>
          {
            // We validate this rule only when there is !IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && !input.IsAlternatingCurrent.Value)
            {
              return inCabinet != null;
            }

            // If Alternating, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field");

      RuleFor(o => o.VoltageOfBattery)
        .Must(
          (input, voltageOfBattery) =>
          {
            // We validate this rule only when there is !IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && !input.IsAlternatingCurrent.Value)
            {
              return voltageOfBattery != null;
            }

            // If IsDirectCurrent, we shouldn't show the error message.
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, voltageOfBattery) =>
            {
              // We validate this rule only when there !IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && !input.IsAlternatingCurrent.Value)
              {
                return input.VoltageOfBatteryMin <= voltageOfBattery && voltageOfBattery <= input.VoltageOfBatteryMax;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 16V and 600V");

      RuleFor(o => o.ArcDurationValue)
        .Must(
          (input, arcDurationValue) =>
          {
            // We validate this rule only when there is !IsAlternatingCurrent
            if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
            {
              return arcDurationValue != null;
            }

            // If IsAlternatingCurrent is null return true
            return true;
          })
        .WithMessage("Required Field")
        .Must(
            (input, arcDurationValue) =>
            {
              // We validate this rule only when there !IsAlternatingCurrent
              if (input.IsAlternatingCurrent.HasValue && input.IsAlternatingCurrent.Value)
              {
                return input.ArcDurationMin <= arcDurationValue && arcDurationValue <= input.ArcDurationMax;
              }

              // If IsAlternatingCurrent is null return true
              return true;
            }
            )
        .WithMessage("Expected between 0.01 and 2 seconds");
    }
  }
}
