using System;
using FluentValidation;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Validator for CreateVehicleCommand.
    /// </summary>
    public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVehicleCommandValidator"/> class.
        /// </summary>
        public CreateVehicleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Vehicle ID is required.")
                .NotNull().WithMessage("Vehicle ID cannot be null.")
                .MaximumLength(50).WithMessage("Vehicle ID must not exceed 50 characters.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .NotNull().WithMessage("Brand cannot be null.")
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required.")
                .NotNull().WithMessage("Model cannot be null.")
                .MaximumLength(100).WithMessage("Model must not exceed 100 characters.");

            RuleFor(x => x.ManufacturingDate)
                .NotNull().WithMessage("Manufacturing date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Manufacturing date cannot be in the future.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddYears(-5)).WithMessage("Vehicle cannot be older than 5 years.");
        }
    }
}
