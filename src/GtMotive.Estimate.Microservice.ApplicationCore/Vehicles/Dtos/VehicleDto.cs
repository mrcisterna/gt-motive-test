using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Dtos
{
    /// <summary>
    /// Vehicle DTO for transferring vehicle data.
    /// </summary>
    public class VehicleDto
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
        public DateTime ManufacturingDate { get; set; }
    }
}
