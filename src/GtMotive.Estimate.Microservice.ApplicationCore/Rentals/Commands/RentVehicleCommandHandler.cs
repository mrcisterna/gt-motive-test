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
    /// Handler for RentVehicleCommand.
    /// </summary>
    public class RentVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        IUnitOfWork unitOfWork,
        IAppLogger<RentVehicleCommandHandler> logger) : IRequestHandler<RentVehicleCommand, RentVehicleCommandResponse>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAppLogger<RentVehicleCommandHandler> _logger = logger;

        /// <summary>
        /// Handles the RentVehicleCommand.
        /// </summary>
        /// <param name="request">Rent vehicle command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Rent vehicle command response.</returns>
        public async Task<RentVehicleCommandResponse> Handle(
            RentVehicleCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            _logger.LogInformation("Processing rental request for vehicle {VehicleId} by renter {RenterId}", request.VehicleId, request.RenterId);

            // Validate vehicle exists and is available
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId).ConfigureAwait(false) ?? throw new VehicleNotFoundException($"Vehicle with ID {request.VehicleId} not found.");
            if (vehicle.Status != VehicleStatus.Available)
            {
                _logger.LogWarning("Vehicle {VehicleId} is not available for rental. Current status: {Status}", request.VehicleId, vehicle.Status);
                throw new DomainException("Vehicle is not available for rental.");
            }

            // Create rental
            var rentalId = Guid.NewGuid().ToString();
            var rental = new Rental(rentalId, request.VehicleId, request.RenterId);

            // Update vehicle status
            vehicle.MarkAsRented(request.RenterId);

            // Save changes
            await _rentalRepository.AddAsync(rental).ConfigureAwait(false);
            await _vehicleRepository.UpdateAsync(vehicle).ConfigureAwait(false);
            await _unitOfWork.Save().ConfigureAwait(false);

            _logger.LogInformation("Rental {RentalId} successfully created for vehicle {VehicleId}", rental.Id, rental.VehicleId);

            return new RentVehicleCommandResponse
            {
                RentalId = rental.Id,
                VehicleId = rental.VehicleId,
                RentalDate = rental.RentalDate
            };
        }
    }
}
