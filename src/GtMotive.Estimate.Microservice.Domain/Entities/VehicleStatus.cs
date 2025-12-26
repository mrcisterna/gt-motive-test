namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Vehicle status enumeration.
    /// </summary>
    public enum VehicleStatus
    {
        /// <summary>
        /// Vehicle is available for rental.
        /// </summary>
        Available = 0,

        /// <summary>
        /// Vehicle is currently rented.
        /// </summary>
        Rented = 1,
    }
}
