using System;

namespace GtMotive.Estimate.Microservice.Api.Dtos
{
    /// <summary>
    /// Response DTO for renting a vehicle.
    /// </summary>
    public class RentVehicleResponseDto
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
