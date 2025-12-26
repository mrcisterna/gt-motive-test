using System;

namespace GtMotive.Estimate.Microservice.Api.Dtos
{
    /// <summary>
    /// Request DTO for creating a vehicle.
    /// </summary>
    public class CreateVehicleRequestDto
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
