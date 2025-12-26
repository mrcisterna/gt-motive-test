using System;

namespace GtMotive.Estimate.Microservice.Api.Dtos
{
    /// <summary>
    /// Response DTO for returning a vehicle.
    /// </summary>
    public class ReturnVehicleResponseDto
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
