namespace GtMotive.Estimate.Microservice.Api.Dtos
{
    /// <summary>
    /// Request DTO for renting a vehicle.
    /// </summary>
    public class RentVehicleRequestDto
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
