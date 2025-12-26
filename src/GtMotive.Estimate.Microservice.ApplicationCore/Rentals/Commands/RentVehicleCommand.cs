using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Command to rent a vehicle.
    /// </summary>
    public class RentVehicleCommand : IRequest<RentVehicleCommandResponse>
    {
        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the renter identifier.
        /// </summary>
        public string RenterId { get; set; }
    }
}
