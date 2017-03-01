using FluentValidation;

namespace ArcFlashCalculator.Core.Model
{
  public class ArcDurationValidator : FluentValidator<ArcDuration>
  {
    public ArcDurationValidator()
    {
      CascadeMode = CascadeMode.StopOnFirstFailure;

      RuleFor(o => o.Manufacturer)
        .NotEmpty()
        .WithMessage("Required Field");  

      RuleFor(o => o.BreakerStyle)
        .NotEmpty()
        .WithMessage("Required Field");  

      RuleFor(o => o.TripUnitType)
        .NotEmpty()
        .WithMessage("Required Field");  

      RuleFor(o => o.LongTimePickup)
        .NotEmpty()
        .WithMessage("Required Field")
        .Must(
            (input, longTimePickup) =>
            {
              // We validate this rule only when there IsAlternatingCurrent and there is a sensor rating value
              if (longTimePickup.HasValue)
              {
                return input.LongTimePickupMin <= longTimePickup && longTimePickup <= input.LongTimePickupMax;
              }

              // If IsDirectCurrent, we shouldn't show the error message.
              return true;
            }
            )
        .WithMessage("Expected between 0.4 and 1");

      RuleFor(o => o.LongTimeDelay)
        .NotEmpty()
        .WithMessage("Required Field");

      RuleFor(o => o.ShortTimePickup)
        .NotEmpty()
        .WithMessage("Required Field");

      RuleFor(o => o.ShortTimeDelay)
        .NotEmpty()
        .WithMessage("Required Field");

      RuleFor(o => o.Instantaneous)
        .NotEmpty()
        .WithMessage("Required Field");
    }
  }
}
