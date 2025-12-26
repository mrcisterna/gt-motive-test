using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Response from ReturnVehicleCommand.
    /// </summary>
    public class ReturnVehicleCommandResponse
    {
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        public string RentalId { get; set; }

        /// <summary>
        /// Gets or sets the return date.
        /// </summary>
        public DateTime ReturnDate { get; set; }
    }
}
