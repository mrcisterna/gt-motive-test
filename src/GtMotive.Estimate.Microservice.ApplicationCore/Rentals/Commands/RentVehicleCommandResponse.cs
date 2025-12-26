using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Response from RentVehicleCommand.
    /// </summary>
    public class RentVehicleCommandResponse
    {
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        public string RentalId { get; set; }

        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the rental date.
        /// </summary>
        public DateTime RentalDate { get; set; }
    }
}
