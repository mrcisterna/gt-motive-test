using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Command to return a rented vehicle.
    /// </summary>
    public class ReturnVehicleCommand : IRequest<ReturnVehicleCommandResponse>
    {
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        public string RentalId { get; set; }
    }
}
