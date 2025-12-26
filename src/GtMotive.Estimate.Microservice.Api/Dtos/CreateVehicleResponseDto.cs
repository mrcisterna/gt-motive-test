namespace GtMotive.Estimate.Microservice.Api.Dtos
{
    /// <summary>
    /// Response DTO for creating a vehicle.
    /// </summary>
    public class CreateVehicleResponseDto
    {
        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the vehicle brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the vehicle model.
        /// </summary>
        public string Model { get; set; }
    }
}
