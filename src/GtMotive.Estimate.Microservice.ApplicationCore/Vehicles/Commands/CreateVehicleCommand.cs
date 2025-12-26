using System;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Command to create a new vehicle.
    /// </summary>
    public class CreateVehicleCommand : IRequest<CreateVehicleCommandResponse>
    {
        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the vehicle brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the vehicle model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the manufacturing date.
        /// </summary>
        public DateTime? ManufacturingDate { get; set; }
    }
}
