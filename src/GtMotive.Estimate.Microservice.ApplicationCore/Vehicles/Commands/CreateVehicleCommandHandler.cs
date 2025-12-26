using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Handler for CreateVehicleCommand.
    /// </summary>
    public class CreateVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        IAppLogger<CreateVehicleCommandHandler> logger) : IRequestHandler<CreateVehicleCommand, CreateVehicleCommandResponse>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAppLogger<CreateVehicleCommandHandler> _logger = logger;

        /// <summary>
        /// Handles the CreateVehicleCommand.
        /// </summary>
        /// <param name="request">Create vehicle command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Create vehicle command response.</returns>
        public async Task<CreateVehicleCommandResponse> Handle(
            CreateVehicleCommand request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            _logger.LogInformation("Creating new vehicle. Brand: {Brand}, Model: {Model}", request.Brand, request.Model);

            var vehicle = new Vehicle(
                request.Id,
                request.Brand,
                request.Model,
                request.ManufacturingDate ?? DateTime.UtcNow);

            await _vehicleRepository.AddAsync(vehicle).ConfigureAwait(false);
            await _unitOfWork.Save().ConfigureAwait(false);

            _logger.LogInformation("Vehicle {VehicleId} successfully created. Brand: {Brand}, Model: {Model}", vehicle.Id, vehicle.Brand, vehicle.Model);

            return new CreateVehicleCommandResponse
            {
                VehicleId = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model
            };
        }
    }
}
