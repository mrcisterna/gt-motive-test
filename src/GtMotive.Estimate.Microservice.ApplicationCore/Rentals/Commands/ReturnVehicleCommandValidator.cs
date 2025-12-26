using FluentValidation;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Validator for ReturnVehicleCommand.
    /// </summary>
    public class ReturnVehicleCommandValidator : AbstractValidator<ReturnVehicleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnVehicleCommandValidator"/> class.
        /// </summary>
        public ReturnVehicleCommandValidator()
        {
            RuleFor(x => x.RentalId)
                .NotEmpty().WithMessage("Rental ID is required.")
                .NotNull().WithMessage("Rental ID cannot be null.")
                .MaximumLength(50).WithMessage("Rental ID must not exceed 50 characters.");
        }
    }
}
