using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Exceptions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Handler for ReturnVehicleCommand.
    /// </summary>
    public class ReturnVehicleCommandHandler(
        IRentalRepository rentalRepository,
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        IAppLogger<ReturnVehicleCommandHandler> logger) : IRequestHandler<ReturnVehicleCommand, ReturnVehicleCommandResponse>
    {
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAppLogger<ReturnVehicleCommandHandler> _logger = logger;

        /// <summary>
        /// Handles the ReturnVehicleCommand.
        /// </summary>
        /// <param name="request">Return vehicle command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Return vehicle command response.</returns>
        public async Task<ReturnVehicleCommandResponse> Handle(
            ReturnVehicleCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            _logger.LogInformation("Processing return request for rental {RentalId}", request.RentalId);

            // Get rental
            var rental = await _rentalRepository.GetByIdAsync(request.RentalId).ConfigureAwait(false) ?? throw new RentalNotFoundException($"Rental with ID {request.RentalId} not found.");
            if (rental.Status != RentalStatus.Active)
            {
                _logger.LogWarning("Rental {RentalId} is not active. Current status: {Status}", request.RentalId, rental.Status);
                throw new DomainException("Rental is not active.");
            }

            // Get vehicle
            var vehicle = await _vehicleRepository.GetByIdAsync(rental.VehicleId).ConfigureAwait(false)
                ?? throw new DomainException($"Vehicle with ID {rental.VehicleId} not found.");

            // Complete rental
            rental.Complete();
            vehicle.MarkAsAvailable();

            // Save changes
            await _rentalRepository.UpdateAsync(rental).ConfigureAwait(false);
            await _vehicleRepository.UpdateAsync(vehicle).ConfigureAwait(false);
            await _unitOfWork.Save().ConfigureAwait(false);

            _logger.LogInformation("Rental {RentalId} successfully completed. Vehicle {VehicleId} marked as available", rental.Id, vehicle.Id);

            return new ReturnVehicleCommandResponse
            {
                RentalId = rental.Id,
                ReturnDate = rental.ReturnDate.Value,
            };
        }
    }
}
