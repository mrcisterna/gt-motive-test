using FluentValidation;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Validator for RentVehicleCommand.
    /// </summary>
    public class RentVehicleCommandValidator : AbstractValidator<RentVehicleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleCommandValidator"/> class.
        /// </summary>
        public RentVehicleCommandValidator()
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("Vehicle ID is required.")
                .NotNull().WithMessage("Vehicle ID cannot be null.")
                .MaximumLength(50).WithMessage("Vehicle ID must not exceed 50 characters.");

            RuleFor(x => x.RenterId)
                .NotEmpty().WithMessage("Renter ID is required.")
                .NotNull().WithMessage("Renter ID cannot be null.")
                .MaximumLength(100).WithMessage("Renter ID must not exceed 100 characters.");
        }
    }
}
